using System;
using System.Web;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net;
using Lucene.Net.Index;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Store;

using System.Threading; 
using System.IO;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Web.Model;
using System.Diagnostics;
using PanGu;

namespace Web
{
    public class IndexManager
    {
        //public static readonly IndexManager bookIndex = new IndexManager();
        // public static readonly string indexPath = HttpContext.Current.Server.MapPath("~/IndexData");

        //private int PageIndex = 1;
        //private int PageSize = 10;
        private IndexWriter writer;
        protected Analyzer pgAnalyzer
        {
            get { return new PanGuAnalyzer(); }
        }
        //protected string IndexDic { get; set; }
        protected Lucene.Net.Store.Directory Lu_IndexDic { get; set; }
            
        
        public IndexManager(string IndexDic = @"../index")
        {
            
            if (!System.IO.Directory.Exists(IndexDic))
            {
                System.IO.Directory.CreateDirectory(IndexDic);
                // Lucene.Net.Store.Directory dd =  Lucene.Net.Store
            }
            Lu_IndexDic = Lucene.Net.Store.FSDirectory.Open(new DirectoryInfo(IndexDic));
            
            //PerFieldAnalyzerWrapper wap = new PerFieldAnalyzerWrapper(new  Lucene.Net.Analysis.Standard.StandardAnalyzer());
            // wap
        }

        public void SetIndexWriter(bool isCreate)
        {
            writer = new IndexWriter(Lu_IndexDic, pgAnalyzer, isCreate, Lucene.Net.Index.IndexWriter.MaxFieldLength.LIMITED);
        }

        public void CreatIndex(bool isCreate)
        {
            //创建索引目录

            
            //IndexWriter第三个参数:true指重新创建索引,false指从当前索引追加....此处为新建索引所以为true
           // IndexWriter writer = 
            for (int i = 1; i < 150; i++)
            {
              //  AddIndex(writer, "嘴嘴" + i.ToString(), "null", DateTime.Now.ToString(), "www.sss");
            }
            writer.Optimize();
            writer.Dispose();
            //Response.Write("<script type='text/javascript'>alert('创建索引成功');window.location=window.location;</script>");
            //Response.End();
        }

        public void SaveIndex()
        {
            writer.Optimize();
            writer.Dispose();
        }


        public void AddIndex(string title, string content, string date, string uri)
        {
            try
            {
                Document doc = new Document();
                doc.Add(new Field("Title", title, Field.Store.YES, Field.Index.ANALYZED));//存储且索引
                doc.Add(new Field("Content", content, Field.Store.YES, Field.Index.ANALYZED));//存储且索引
                doc.Add(new Field("AddTime", date, Field.Store.YES, Field.Index.NOT_ANALYZED));//存储且索引
                doc.Add(new Field("Uri", uri, Field.Store.YES, Field.Index.NOT_ANALYZED));
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

        public void SearchIndex(string st,Page pg)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            BooleanQuery bQuery = new BooleanQuery();
            if (st != null && st != "")
            {
                //title = GetKeyWordsSplitBySpace(Request.Form["title"].ToString());

                //QueryParser parseTitle = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Title", pgAnalyzer);
                //parseTitle.DefaultOperator = QueryParser.Operator.AND;
                //Query queryT = parseTitle.Parse(GetKeyWordsSplitBySpace(st));
                //bQuery.Add(queryT, Occur.MUST);


                QueryParser parseContent = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Content", pgAnalyzer);
                parseContent.DefaultOperator = QueryParser.Operator.OR;
                Query queryC = parseContent.Parse(GetKeyWordsSplitBySpace(st));
                bQuery.Add(queryC, Occur.MUST);


                dic.Add("title", st);
                // txtTitle = Request.Form["title"].ToString();
            }
            //if (Request.Form["content"] != null && Request.Form["content"].ToString() != "")
            //{
            //    content = GetKeyWordsSplitBySpace(Request.Form["content"].ToString());
            //    QueryParser parse = new QueryParser("Content", PanGuAnalyzer);
            //    Query query = parse.Parse(content);
            //    parse.SetDefaultOperator(QueryParser.Operator.AND);
            //    bQuery.Add(query, BooleanClause.Occur.MUST);
            //    dic.Add("content", Request.Form["content"].ToString());
            //    txtContent = Request.Form["content"].ToString();
            //}
            if (bQuery != null && bQuery.GetClauses().Length > 0)
            {
                GetSearchResult(bQuery, dic,pg);
            }
        }

        private void GetSearchResult(BooleanQuery bQuery, Dictionary<string, string> dicKeywords,Page pg)
        {
            List<Record> returnA = new List<Record>();
            //Lucene.Net.Store.Directory ad = Lucene.Net.Store.FSDirectory.Open(new DirectoryInfo(IndexDic));
            IndexSearcher search = new IndexSearcher(Lu_IndexDic, true);
            Stopwatch stopwatch = Stopwatch.StartNew();

            Sort sort = new Sort(new SortField("Title", SortField.DOC, true));
            TopDocs docs = search.Search(bQuery, (Filter)null, pg.PageSize * pg.PageIndex, sort);
            stopwatch.Stop();
            if (docs != null && docs.TotalHits > 0)
            {
                // lSearchTime = stopwatch.ElapsedMilliseconds;
                //txtPageFoot = GetPageFoot(PageIndex, PageSize, docs.totalHits, "sabrosus");
                for (int i = 0; i < docs.TotalHits; i++)
                {
                    if (i >= (pg.PageIndex - 1) * pg.PageSize && i < pg.PageIndex * pg.PageSize)
                    {
                        Document doc = search.Doc(docs.ScoreDocs[i].Doc);
                        Record model = new Record()
                        {
                            Title = doc.Get("Title").ToString(),
                            Content = doc.Get("Content").ToString(),
                            AddTime = doc.Get("AddTime").ToString(),
                            Uri = doc.Get("Uri").ToString()
                        };

                        Console.WriteLine(model.Title +"    "+model.Uri);
                    returnA.Add(model);
                    //list.Add(SetHighlighter(dicKeywords, model));
                    }
                }
            }
        }
        

        private string GetKeyWordsSplitBySpace(string keywords)
        {
            PanGuTokenizer ktTokenizer = new PanGuTokenizer();
            StringBuilder result = new StringBuilder();
            ICollection<WordInfo> words = ktTokenizer.SegmentToWordInfos(keywords);
            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }
                result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
            }
            return result.ToString().Trim();
        }


        //请求队列 解决索引目录同时操作的并发问题
        private Queue<BookViewMode> bookQueue = new Queue<BookViewMode>();
        /// <summary>
        /// 新增Books表信息时 添加邢增索引请求至队列
        /// </summary>
        /// <param name="books"></param>
        //public void Add(Books books)
        //{
        //    BookViewMode bvm = new BookViewMode();
        //    bvm.Id = books.Id;
        //    bvm.Title = books.Title;
        //    bvm.IT = IndexType.Insert;
        //    bvm.Content = books.ContentDescription;
        //    bookQueue.Enqueue(bvm);
        //}
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
        //public void Mod(Books books)
        //{
        //    BookViewMode bvm = new BookViewMode();
        //    bvm.Id = books.Id;
        //    bvm.Title = books.Title;
        //    bvm.IT = IndexType.Modify;
        //    bvm.Content = books.ContentDescription;
        //    bookQueue.Enqueue(bvm);
        //}

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
                    //CRUDIndex();
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
        //    private void CRUDIndex()
        //    {
        //        FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
        //        bool isExist = IndexReader.IndexExists(directory);
        //        if (isExist)
        //        {
        //            if (IndexWriter.IsLocked(directory))
        //            {
        //                IndexWriter.Unlock(directory);
        //            }
        //        }

        //        IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);
        //        while (bookQueue.Count > 0)
        //        {
        //            Document document = new Document();
        //            BookViewMode book = bookQueue.Dequeue();
        //            if (book.IT == IndexType.Insert)
        //            {
        //                document.Add(new Field("id", book.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        //                document.Add(new Field("title", book.Title, Field.Store.YES, Field.Index.ANALYZED,
        //                                       Field.TermVector.WITH_POSITIONS_OFFSETS));
        //                document.Add(new Field("content", book.Content, Field.Store.YES, Field.Index.ANALYZED,
        //                                       Field.TermVector.WITH_POSITIONS_OFFSETS));
        //                writer.AddDocument(document);
        //            }
        //            else if (book.IT == IndexType.Delete)
        //            {
        //                writer.DeleteDocuments(new Term("id", book.Id.ToString()));
        //            }
        //            else if (book.IT == IndexType.Modify)
        //            {
        //                //先删除 再新增
        //                writer.DeleteDocuments(new Term("id", book.Id.ToString()));
        //                document.Add(new Field("id", book.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        //                document.Add(new Field("title", book.Title, Field.Store.YES, Field.Index.ANALYZED,
        //                                       Field.TermVector.WITH_POSITIONS_OFFSETS));
        //                document.Add(new Field("content", book.Content, Field.Store.YES, Field.Index.ANALYZED,
        //                                       Field.TermVector.WITH_POSITIONS_OFFSETS));
        //                writer.AddDocument(document);
        //            }
        //        }
        //        writer.Dispose();
        //        directory.Dispose();
        //    }
        //}
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

        public class Page
        {
            public int PageSize { get; set; }
            public int PageIndex { get; set; }
        }
    }

}
