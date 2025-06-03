#!/bin/bash

# Migration script for ComputerAPI
echo "Creating initial migration..."
dotnet ef migrations add InitialCreate

echo "Applying migration to database..."
dotnet ef database update

echo "Migration complete!"
