-- Database Creation
USE master;
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'BookingDB')
    DROP DATABASE BookingDB;
CREATE DATABASE BookingDB;

USE BookingDB;

-- Table: Venue
CREATE TABLE Venue (
    VenueId INT IDENTITY(1,1) PRIMARY KEY,
    VenueName VARCHAR(255) NOT NULL,
    Location VARCHAR(255),
    Capacity INT,
    ImageUrl VARCHAR(500)
);

-- Table: Event
CREATE TABLE Event (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    EventName VARCHAR(255) NOT NULL,
    EventDate DATE,
    Description TEXT,
    VenueId INT,
    FOREIGN KEY (VenueId) REFERENCES Venue(VenueId)
);

-- Table: Booking (Associative table)
CREATE TABLE Booking (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT,
    VenueId INT,
    BookingDate DATE,
    FOREIGN KEY (EventId) REFERENCES Event(EventId),
    FOREIGN KEY (VenueId) REFERENCES Venue(VenueId)
);
