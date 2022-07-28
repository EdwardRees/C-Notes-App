DROP TABLE IF EXISTS Note;
DROP TABLE IF EXISTS Collection;
CREATE TABLE Collection(id serial primary key, name text);
CREATE TABLE Note(id serial primary key, collectionId numeric, title text, description text, tag varchar(64), pinned bool);
