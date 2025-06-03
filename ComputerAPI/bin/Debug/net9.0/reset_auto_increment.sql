-- Script to reset auto-increment values to start from 1
-- Execute this after running migrations

-- Reset auto-increment for user_claims table
ALTER TABLE `user_claims` AUTO_INCREMENT = 1;

-- Reset auto-increment for role_claims table
ALTER TABLE `role_claims` AUTO_INCREMENT = 1;

-- Any other tables with auto-increment IDs can be added here
