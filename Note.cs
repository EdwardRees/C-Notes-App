using System;

namespace NotesApp {
	public class Note {
		private int id;
		private string title;
		private string desc;
		private string tag;
		private bool pinned;
		private static int counts = 0;

		public Note(int id, string title, string desc, string tag, bool pinned){
			this.id = id;
			this.title = title;
			this.desc = desc;
			this.tag = tag;
			this.pinned = pinned;
			counts++;
		}

		public int GetId(){
			return this.id;
		}
		public string GetTitle(){
			return this.title;
		}
		public string GetDesc(){
			return this.desc;
		}
		public string GetTag(){
			return this.tag;
		}
		public bool IsPinned(){
			return this.pinned;
		}
		
		public override string ToString(){
			return String.Format("[{0}: \"{3}\"] {1}: {2}", this.id, this.title, this.desc, this.tag);
		}
	}
}
