CREATE TABLE test_table2(
                     id serial PRIMARY KEY,
                     num  SMALLINT NOT NULL,
                     opt SMALLINT,
                     arr SMALLINT[],
                     arrStr VARCHAR(255)[],
                     info json,
                     bobo boolean 
                    ); 

CREATE TABLE test_table(
                     id serial PRIMARY KEY,
                     title VARCHAR(255) UNIQUE NOT NULL,
                     created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                     ); 

