using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

namespace TableDriver
{
	class Schema
	{
		public static bool Exists( string in_schemaname, DbConfig in_config ) {
			IDbConnection conn = ConnectionFactory.GetConnection( in_config );
			using( conn ) {
				IDataReader reader = DbUtil.ExecuteReader( conn, "IF EXISTS (SELECT * FROM sys.schemas WHERE name = '" + in_schemaname + "') select 1" );
				if( reader.Read() ) {
					return true;
				}
				else {
					return false;
				}
			}
		}
		public static void Create( string in_schemaname, DbConfig in_config ) {
			IDbConnection conn = ConnectionFactory.GetConnection( in_config );
			using( conn ) {
				if( !Exists( in_schemaname, in_config ) ) {
					IDataReader reader = DbUtil.ExecuteReader( conn, "create schema " + in_schemaname );
				}
			}
		}
	}
}
