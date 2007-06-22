DROP TABLE IF EXISTS persons;
DROP TABLE IF EXISTS events;
DROP TABLE IF EXISTS event_types;
DROP TABLE IF EXISTS attendances;
DROP TABLE IF EXISTS instruments;
DROP TABLE IF EXISTS play_what;

CREATE TABLE persons (
	/* Basic data */
	id integer primary key autoincrement,
	dni integer,
	surname varchar(25) not null,
	name varchar(25) not null,
	is_man integer not null,
	birthday_date varchar(30),

	/* Domicilio */
	address varchar(40),
	city varchar(25),

	/* Contacto */
	land_phone_number varchar(20),
	mobile_number varchar(20),
	email varchar(35),

	/* Relacionados al coro */
	comunity varchar(20),
	is_active integer not null,

	/* Si los datos de la persona están completos */
	is_data_complete integer not null,

	/* dni es unico */
	unique (dni)
);

CREATE TABLE events (
	date varchar(30) primary key,
	name varchar(30) not null,
	id_event_type integer not null,
	goals varchar(50),
	observations varchar(50)
);

CREATE TABLE event_types (
	id integer primary key autoincrement,
	name varchar(20) not null,

	/* nombre es único */
	unique(name)
);

CREATE TABLE attendances (
	id_person integer,
	event_date varchar(30),
	primary key(id_person, event_date)
);

CREATE TABLE instruments (
	name varchar(30) primary key
);

CREATE TABLE play_what (
	dni varchar(10) not null,
	instrument_name varchar(30) not null,
	primary key (dni, instrument_name)
);

INSERT INTO event_types(name) values('Misa');
INSERT INTO event_types(name) values('Ensayo');
INSERT INTO event_types(name) values('Otro');

