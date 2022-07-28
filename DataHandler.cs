using System.Collections.Generic;

namespace NotesApp {
  public class DataHandler {
    private SQLConnector connector;
    private List<NoteCollection> collections;
    public DataHandler(){
      this.connector = new SQLConnector("127.0.0.1", "guest", "password", "csnotesdb");
      this.collections = new List<NoteCollection>();
    }

    public void GenerateCollections(){
      Dictionary<string, List<string>> CollectionResults = this.connector.ExecuteReadQuery("SELECT * FROM Collection;");
      foreach(KeyValuePair<string, List<string>> kvp in CollectionResults){
        if(kvp.Key.Equals("headers")){
          continue;
        }
        this.collections.Add(new NoteCollection(int.Parse(kvp.Value[0]), kvp.Value[1]));
      }
    }
    public List<NoteCollection> GetCollections(){
      return this.collections;
    }

    public void GetNotes(){
      Dictionary<string, List<string>> NoteResults = this.connector.ExecuteReadQuery("SELECT * FROM Note;");
      foreach(KeyValuePair<string, List<string>> kvp in NoteResults){
        if(kvp.Key.Equals("headers")){
          continue;
        }
        List<string> row = kvp.Value;
        Note note = new Note(int.Parse(row[0]), row[2], row[3], row[4], bool.Parse(row[5]));
        for(int i=0; i<this.collections.Count; i++){
          if(this.collections[i].GetId() == int.Parse(row[1])){
            this.collections[i].Add(note);
            break;
          }
        }
      }
    }

    public void CloseConnection(){
      this.connector.CloseConnection();
    }


  }
}
