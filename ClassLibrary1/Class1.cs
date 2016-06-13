using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Dapper;
using NSubstitute;

namespace ClassLibrary1
{
    public class Class1
    {

        public class TestParameter :IDbDataParameter
        {
            public DbType DbType { get; set; }
            public ParameterDirection Direction { get; set; }
            public bool IsNullable { get; }
            public string ParameterName { get; set; }
            public string SourceColumn { get; set; }
            public DataRowVersion SourceVersion { get; set; }
            public object Value { get; set; }
            public byte Precision { get; set; }
            public byte Scale { get; set; }
            public int Size { get; set; }
        }

        public class TestCommand : IDbCommand
        {

            IDataParameterCollection _internalCollection = new TestDataParamCollection();
            public void Dispose()
            {
               
            }

            public void Prepare()
            {
                throw new NotImplementedException();
            }

            public void Cancel()
            {
                throw new NotImplementedException();
            }

            public IDbDataParameter CreateParameter()
            {
                return new TestParameter();
            }

            public int ExecuteNonQuery()
            {
               Console.WriteLine(this.CommandText);
                return 1;
            }

            public IDataReader ExecuteReader()
            {
                throw new NotImplementedException();
            }

            public IDataReader ExecuteReader(CommandBehavior behavior)
            {
                throw new NotImplementedException();
            }

            public object ExecuteScalar()
            {
                throw new NotImplementedException();
            }

            public IDbConnection Connection { get; set; }
            public IDbTransaction Transaction { get; set; }
            public string CommandText { get; set; }
            public int CommandTimeout { get; set; }
            public CommandType CommandType { get; set; }

            public IDataParameterCollection Parameters => _internalCollection;

            public UpdateRowSource UpdatedRowSource { get; set; }
        }

        public class TestDataParamCollection:IDataParameterCollection
        {
            List<object> _internalList = new List<object>();
            public IEnumerator GetEnumerator()
            {
                return _internalList.GetEnumerator();
            }

            public void CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            public int Count { get; }
            public object SyncRoot { get; }
            public bool IsSynchronized { get; }
            public int Add(object value)
            {
                _internalList.Add(value);
                return 1;
            }

            public bool Contains(object value)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
               _internalList.Clear();
            }

            public int IndexOf(object value)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, object value)
            {
                throw new NotImplementedException();
            }

            public void Remove(object value)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            object IList.this[int index]
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public bool IsReadOnly { get; }
            public bool IsFixedSize { get; }
            public bool Contains(string parameterName)
            {
                throw new NotImplementedException();
            }

            public int IndexOf(string parameterName)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(string parameterName)
            {
                throw new NotImplementedException();
            }

            object IDataParameterCollection.this[string parameterName]
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
        }

        public class TestConnection : IDbConnection
        {
            public void Dispose()
            {
               
            }

            public IDbTransaction BeginTransaction()
            {
                throw new NotImplementedException();
            }

            public IDbTransaction BeginTransaction(IsolationLevel il)
            {
                throw new NotImplementedException();
            }

            public void Close()
            {
                
            }

            public void ChangeDatabase(string databaseName)
            {
                throw new NotImplementedException();
            }

            public IDbCommand CreateCommand()
            {
                return new TestCommand();
            }

            public void Open()
            {
               
            }

            public string ConnectionString { get; set; }
            public int ConnectionTimeout { get; }
            public string Database { get; }
            public ConnectionState State { get; }
        }
        [Test]
        public void test1()
        {
            
             var myConnection = new TestConnection();
             myConnection.Execute("select * @test",new {test="te"});

        }

        [Test]
        public void Should_call_db_wiht_right_param()
        {
            var dbConnectionFactory = Substitute.For<IDbConnectionFactory>();
            dbConnectionFactory.GetConnection(Arg.Any<string>()).Returns(new TestConnection());
            var sut = new TestRepository(dbConnectionFactory);
            sut.DoInsert();
        }
    }


    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection(string connectionString);
    }

    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
    public class TestRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public TestRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public void DoInsert()
        {
            using (var conne = _dbConnectionFactory.GetConnection("test"))
            {
                conne.Execute("select * from test where id = @id", new {id = "1234"});


            }
        }
    }
}
