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

    public NoteCollection GetCollectionById(int id){
      for(int i=0; i<this.collections.Count; i++){
        if(this.collections[i].GetId() == id){
          return this.collections[i];
        }
      }
      return null;
    }

    public void NewNote(int id, int collectionId, string title, string desc, string tag, bool pinned){
      NoteCollection col = GetCollectionById(collectionId);
      col.Add(new Note(id, title, desc, tag, pinned));
    }

    public void UpdateDatabase(){
      this.connector.ExecuteNonQuery("DROP TABLE IF EXISTS Collection;");
      this.connector.ExecuteNonQuery("DROP TABLE IF EXISTS Collection_id_seq;");
      this.connector.ExecuteNonQuery("DROP TABLE IF EXISTS Note;");
      this.connector.ExecuteNonQuery("DROP TABLE IF EXISTS Note_id_seq;");
      this.connector.ExecuteNonQuery("CREATE TABLE Collection(id serial primary key, name text);");
      this.connector.ExecuteNonQuery("CREATE TABLE Note(id serial primary key, collectionId numeric, title text, description text, tag varchar(64), pinned bool);");
      for(int i=0; i<this.collections.Count; i++){
        NoteCollection col = this.collections[i];
        this.connector.ExecuteNonQuery(String.Format("INSERT INTO Collection VALUES ({0}, '{1}');", col.GetId(), col.GetName()));
        for(int j=0; j<col.Size(); j++){
          Note note = col.Get(j);
          this.connector.ExecuteNonQuery(String.Format("INSERT INTO Note VALUES ({0}, {1}, '{2}', '{3}', '{4}', {5});", note.GetId(), col.GetId(), note.GetTitle(), note.GetDesc(), note.GetTag(), note.IsPinned()));
        }
      }
    }

    public void CloseConnection(){
      this.connector.CloseConnection();
    }


  }
}
