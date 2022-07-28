using Npgsql;
using System.Text;
using System.Collections.Generic;

namespace NotesApp {
	public class SQLConnector {
		private string host;
		private string username;
		private string password;
		private string database;
		private NpgsqlConnection connector;

		public SQLConnector(string host, string username, string password, string database){
			this.host = host;
			this.username = username;
			this.password = password;
			this.database = database;
			this.SetupConnection();
		}
		public void SetupConnection(){
			var cs = String.Format("Host={0};Username={1};Password={2};Database={3}", this.host, this.username, this.password, this.database);
			connector = new NpgsqlConnection(cs);
			connector.Open();
		}

		public void ExecuteNonQuery(string query){
			NpgsqlCommand cmd = new NpgsqlCommand(query, connector);
			cmd.ExecuteNonQuery();
		}

		public void ExecutePrintReadQuery(string query){
			NpgsqlCommand cmd = new NpgsqlCommand(query, connector);
			using NpgsqlDataReader rdr = cmd.ExecuteReader();
      StringBuilder header = new StringBuilder();
      for(int i=0; i<rdr.FieldCount; i++){
        header.Append(String.Format("{0,-10} ", rdr.GetName(i)));
      }
      Console.WriteLine(header);
			while(rdr.Read()){
				StringBuilder sb = new StringBuilder();
				int count = rdr.FieldCount;
				for(int i=0; i<count; i++){
           switch(String.Format("{0}", rdr.GetFieldType(i))){
						case "System.Int32":
							sb.Append(String.Format("{0,-10} ", rdr.GetInt32(i)));
							break;
						case "System.Decimal":
							sb.Append(String.Format("{0,-10} ", rdr.GetDecimal(i)));
							break;
						case "System.String":
							sb.Append(String.Format("{0,-10} ", rdr.GetString(i)));
							break;
            case "System.Boolean":
              sb.Append(String.Format("{0,-10} ", rdr.GetBoolean(i)));
              break;
						default:
							break;
					}
				}
				Console.WriteLine(sb.ToString());
			}
		}
    public Dictionary<string, List<string>> ExecuteReadQuery(string query){
      Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();
      NpgsqlCommand cmd = new NpgsqlCommand(query, connector);
      using NpgsqlDataReader rdr = cmd.ExecuteReader();
      List<string> headers = new List<string>();
      for(int i=0; i<rdr.FieldCount; i++){
        headers.Add(String.Format("{0}", rdr.GetName(i)));
      }
      results.Add("headers", headers);
      int row = 0;
      while(rdr.Read()){
        List<string> rowData = new List<string>();
        for(int i=0; i<rdr.FieldCount; i++){
          switch(String.Format("{0}", rdr.GetFieldType(i))){
            case "System.Int32":
              rowData.Add(String.Format("{0}", rdr.GetInt32(i)));
              break;
            case "System.Decimal":
              rowData.Add(String.Format("{0}", rdr.GetDecimal(i)));
              break;
            case "System.String":
              rowData.Add(rdr.GetString(i));
              break;
            case "System.Boolean":
              rowData.Add(String.Format("{0}", rdr.GetBoolean(i)));
              break;
            default:
              break;
          }
        }
        results.Add(String.Format("Row {0}", row), rowData);
        row++;
      }
      return results;
    }
		public void CloseConnection(){
			connector.Close();
		}
	}
}
