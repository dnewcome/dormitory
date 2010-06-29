using System;
using System.Configuration;

namespace TableDriver
{
	/// <summary>
	/// Configuration class for the table driver.  Default constructor pulls config data
	/// from app.config.  Specialized constructor takes the values in as constructor arguments.
	/// Once configured, the config cannot be changed.  This data structure is designed to be 
	/// passed around as the authoritative configuration data for any mapper instances that 
	/// reference it.
	/// Note that this code was factored out of the Dynamic ORM, so we called it DbConfig -
	/// TODO: change this to just Config, or maybe TDConfig? Right now Dormitory doesn't need
	/// any additional configuration, but DynamoicORM does.
	/// </summary>
	public class DbConfig
	{
		// default constructor - use values from app.config
		public DbConfig() {
			m_connectionString = ConfigurationManager.AppSettings[ "connectionString" ];
			m_schema = ConfigurationManager.AppSettings[ "schema" ];
			m_databaseType = ( DatabaseType )Enum.Parse(
				typeof( DatabaseType ), ConfigurationManager.AppSettings[ "databaseType" ]
			);
		}

		// constructor: sets up config based on values passed to constructor instead of using
		// values from config file
		public DbConfig(
			string in_connectionString,
			string in_mapperSchema,
			DatabaseType in_databaseType )
		{
			if( in_connectionString == null || in_mapperSchema == null )
			{
				throw new Exception( "Invalid configuration arguments passed to MapperConfig" );
			}
			m_connectionString = in_connectionString;
			m_databaseType = in_databaseType;
		}

		// the MapperSchema is the SQL server schema that is used to track the object
		// definitions, this is the schema that stores our mapper schema.
		public string Schema
		{
			get { return m_schema; }
		} private string m_schema;

		// the sql server connection string to use to connect to the database
		public string ConnectionString
		{
			get { return m_connectionString; }
		} private string m_connectionString;

		// the sql server connection string to use to connect to the database
		public DatabaseType DatabaseType
		{
			get { return m_databaseType; }
		} private DatabaseType m_databaseType;
	}
}