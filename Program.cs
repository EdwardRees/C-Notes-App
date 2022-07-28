using System;
namespace NotesApp {
  public class Program {
    private static void TestStructures(){
      NoteCollection ToDoNotes = new NoteCollection(0, "To Do Notes");
      ToDoNotes.Add(new Note(0, "Get Groceries", "Go buy groceries", "Grocery todos", false));
      ToDoNotes.Add(new Note(1, "Run", "Go for a run", "Exercise todos", false));
      Console.WriteLine(ToDoNotes);
    }
    private static void TestSQL(){
      SQLConnector conn = new SQLConnector("127.0.0.1", "guest", "password", "csnotesdb");
      conn.ExecutePrintReadQuery("SELECT * FROM Note;");
      Console.WriteLine();
      conn.ExecutePrintReadQuery("SELECT * FROM Collection;");
      Console.WriteLine();
      Console.WriteLine(conn.ExecuteReadQuery("SELECT * FROM Note;"));
      conn.CloseConnection();
    }

    private static void TestDataHandler(){
      DataHandler handler = new DataHandler();
      handler.GenerateCollections();

      handler.GetNotes();
      foreach(NoteCollection col in handler.GetCollections()){
        Console.WriteLine(col);
      }
      handler.CloseConnection();
    }
    public static void Main(String[] args){
      TestDataHandler();
    }
  }
}
