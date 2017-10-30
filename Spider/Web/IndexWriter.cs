using System;
using System.Web;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net;
using Lucene.Net.Index;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Store;

using System.Threading; 
using System.IO;

namespace Web
{
    public class IndexManager
    {
        public static readonly IndexManager bookIndex = new IndexManager();
        public static readonly string indexPath = HttpContext.Current.Server.MapPath("~/IndexData");
        protected string IndexDic
        {
            get
            {
                return  "/IndexDic";
            }
        }
        private IndexManager()
        {
            
            //PerFieldAnalyzerWrapper wap = new PerFieldAnalyzerWrapper(new  Lucene.Net.Analysis.Standard.StandardAnalyzer());
           // wap
        }

        void CreatIndex()
        {
            //创建索引目录
            if (!System.IO.Directory.Exists(IndexDic))
            {
                System.IO.Directory.CreateDirectory(IndexDic);
            }
            //IndexWriter第三个参数:true指重新创建索引,false指从当前索引追加....此处为新建索引所以为true
            IndexWriter writer = new IndexWriter(IndexDic, PanGuAnalyzer, isCreate, Lucene.Net.Index.IndexWriter.MaxFieldLength.LIMITED);
            for (int i = 1; i < 150; i++)
            {
              
            }
            writer.Optimize();
            writer.Close();
            Response.Write("<script type='text/javascript'>alert('创建索引成功');window.location=window.location;</script>");
            Response.End();
        }



        private void AddIndex(IndexWriter writer, string title, string content, string date)
        {
            try
            {
                Document doc = new Document();
                doc.Add(new Field("Title", title, Field.Store.YES, Field.Index.NOT_ANALYZED));//存储且索引
                doc.Add(new Field("Content", content, Field.Store.YES, Field.Index.ANALYZED));//存储且索引
                doc.Add(new Field("AddTime", date, Field.Store.YES, Field.Index.NOT_ANALYZED));//存储且索引
                writer.AddDocument(doc);
            }
            catch (FileNotFoundException fnfe)
            {
                throw fnfe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //请求队列 解决索引目录同时操作的并发问题
        private Queue<BookViewMode> bookQueue = new Queue<BookViewMode>();
        /// <summary>
        /// 新增Books表信息时 添加邢增索引请求至队列
        /// </summary>
        /// <param name="books"></param>
        public void Add(Books books)
        {
            BookViewMode bvm = new BookViewMode();
            bvm.Id = books.Id;
            bvm.Title = books.Title;
            bvm.IT = IndexType.Insert;
            bvm.Content = books.ContentDescription;
            bookQueue.Enqueue(bvm);
        }
        /// <summary>
        /// 删除Books表信息时 添加删除索引请求至队列
        /// </summary>
        /// <param name="bid"></param>
        public void Del(int bid)
        {
            BookViewMode bvm = new BookViewMode();
            bvm.Id = bid;
            bvm.IT = IndexType.Delete;
            bookQueue.Enqueue(bvm);
        }
        /// <summary>
        /// 修改Books表信息时 添加修改索引(实质上是先删除原有索引 再新增修改后索引)请求至队列
        /// </summary>
        /// <param name="books"></param>
        public void Mod(Books books)
        {
            BookViewMode bvm = new BookViewMode();
            bvm.Id = books.Id;
            bvm.Title = books.Title;
            bvm.IT = IndexType.Modify;
            bvm.Content = books.ContentDescription;
            bookQueue.Enqueue(bvm);
        }

        public void StartNewThread()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(QueueToIndex));
        }

        //定义一个线程 将队列中的数据取出来 插入索引库中
        private void QueueToIndex(object para)
        {
            while (true)
            {
                if (bookQueue.Count > 0)
                {
                    CRUDIndex();
                }
                else
                {
                    Thread.Sleep(3000);
                }
            }
        }
        /// <summary>
        /// 更新索引库操作
        /// </summary>
        private void CRUDIndex()
        {
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            bool isExist = IndexReader.IndexExists(directory);
            if (isExist)
            {
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);
            while (bookQueue.Count > 0)
            {
                Document document = new Document();
                BookViewMode book = bookQueue.Dequeue();
                if (book.IT == IndexType.Insert)
                {
                    document.Add(new Field("id", book.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", book.Title, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("content", book.Content, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    writer.AddDocument(document);
                }
                else if (book.IT == IndexType.Delete)
                {
                    writer.DeleteDocuments(new Term("id", book.Id.ToString()));
                }
                else if (book.IT == IndexType.Modify)
                {
                    //先删除 再新增
                    writer.DeleteDocuments(new Term("id", book.Id.ToString()));
                    document.Add(new Field("id", book.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", book.Title, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("content", book.Content, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    writer.AddDocument(document);
                }
            }
            writer.Dispose();
            directory.Dispose();
        }
    }
    public class BookViewMode
    {
        public int Id
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Content
        {
            get;
            set;
        }
        public IndexType IT
        {
            get;
            set;
        }
    }
    //操作类型枚举
    public enum IndexType
    {
        Insert,
        Modify,
        Delete
    }


}
