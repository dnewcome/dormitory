using System;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Djn.Testing;

namespace TableDriver
{
	class Tests
	{
		[FestTest]
		public void TestSchema() {
			DbConfig config = new DbConfig();
			TableDriver.Schema.Create( "test", config );
			Fest.AssertTrue( TableDriver.Schema.Exists( "test", config ), "Schema does not exist" );
		}

		[FestTest]
		public void TestTable() {
			DbConfig config = new DbConfig();
			List<Field> fields = new List<Field>(){ new Field( "col1", typeof(string) ) };
			TableDriver.Table.CreateTable( "testtable", fields, config );
			Fest.AssertTrue( TableDriver.Table.Exists( "test", "testtable", config ), "Table does not exist" );

			TableDriver.Table.Drop( "test", "testtable", config );
			Fest.AssertFalse( TableDriver.Table.Exists( "test", "testtable", config ), "Table exists" );
		}

		public static void Main() {
			Fest.Run();
			Console.ReadLine();
		}
	}
}
