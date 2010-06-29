using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;

namespace TableDriver
{
	/**
	 * Database utility class to create database connections
	 * according to the connection string contained in our 
	 * mapper config file
	 */
	public class ConnectionFactory
	{ 
		/**
		 * Return a database connection according to mapper config
		 * could be oracle or sql server
		 * 
		 * @in_mapperConfig - mapper configuration object
		 */
		public static IDbConnection GetConnection( DbConfig in_mapperConfig )
		{
			IDbConnection returnValue;
			if( in_mapperConfig.DatabaseType == DatabaseType.MSSql ) {
				returnValue = new SqlConnection( in_mapperConfig.ConnectionString );
			}
			else if( in_mapperConfig.DatabaseType == DatabaseType.Oracle ) {
				returnValue = new OracleConnection( in_mapperConfig.ConnectionString );
			}
			else { throw new Exception( "Unsupported database type" ); }
			return returnValue;
		}
	}
}