FROM postgres:11.5-alpine
COPY ./start-up.sh /docker-entrypoint-initdb.d/
RUN chmod +x /docker-entrypoint-initdb.d/start-up.sh





