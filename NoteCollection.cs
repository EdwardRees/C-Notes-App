using System;
using System.Collections.Generic;
using System.Text;
namespace NotesApp {
	public class NoteCollection {
		private int id;
		private string name;
		private List<Note> notes;

		public NoteCollection(int id, string name){
			this.id = id;
			this.name = name;
			this.notes = new List<Note>();
		}
    
    public string GetName(){
      return this.name;
    }
    public int GetId(){
      return this.id;
    }

		public void Add(Note note){
			this.notes.Add(note);
		}

		public Note Remove(int index){
			if(index > this.notes.Count || index < 0){
				throw new IndexOutOfRangeException();
			}
			Note note = this.notes[index];
			this.notes.RemoveAt(index);
			return note;
		}

		public Note Get(int index){
			if(index > this.notes.Count || index < 0){
				throw new IndexOutOfRangeException();
			}
			return this.notes[index];
		}

		public Note Set(int index, Note note){
			if(index > this.notes.Count || index < 0){
				throw new IndexOutOfRangeException();
			}
			this.notes[index] = note;
			return this.notes[index];
		}

		public void PrintCollection(){
			Console.WriteLine(this.ToString());
		}


		public override string ToString(){
			StringBuilder sb = new StringBuilder();
			sb.Append(this.name + "\n");
			for(int i=0; i<this.notes.Count; i++){
				sb.Append(this.notes[i]);
				sb.Append("\n");
			}
			return sb.ToString().Trim();
		}
		public List<Note> GetPinned(){
			List<Note> pinnedNotes = new List<Note>();
			for(int i=0; i<this.notes.Count; i++){
				if(this.notes[i].IsPinned()){
					pinnedNotes.Add(this.notes[i]);
				}
			}
			return pinnedNotes;
		}
		public List<Note> GetByTag(string tag){
			List<Note> taggedNotes = new List<Note>();
			for(int i=0; i<this.notes.Count; i++){
				if(this.notes[i].GetTag().Equals(tag)){
					taggedNotes.Add(this.notes[i]);
				}
			}
			return taggedNotes;
		}
	}
}
