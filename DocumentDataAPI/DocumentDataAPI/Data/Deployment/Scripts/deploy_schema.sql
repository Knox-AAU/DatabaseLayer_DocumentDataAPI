-- Creates a copy of the document_data schema (see https://wiki.knox.cs.aau.dk/en/Database/DocumentDataAPI/document_data).
-- The actual name of the schema is provided though the ${schema} placeholder.
-- Also creates the readonly and read_write users that are used in production (but not necessarily during local development)

drop schema if exists ${schema} cascade;

create schema ${schema};

create table ${schema}.data_sources (
    id          bigserial primary key,
    name        varchar(100) not null
);

create index data_sources_name_idx on ${schema}.data_sources (name);

create table ${schema}.categories (
    category_id     serial not null,
    name            varchar(100),
    constraint pk_categories primary key (category_id)
);

create index categories_name_idx on ${schema}.categories (name);

create table ${schema}.documents (
    id              bigint primary key,
    sources_id      bigint not null,
    categories_id   bigint not null,
    publication     varchar(100),
    title           varchar(400) not null,
    path            varchar(400) not null,
    summary         text,
    date            timestamp not null,
    author          varchar(200) not null,
    total_words     bigint not null,
    unique_words    bigint not null,
    constraint fk_data_sources foreign key (sources_id) references ${schema}.data_sources(id),
    constraint fk_categories foreign key (categories_id) references ${schema}.categories
);

create index documents_sources_id_idx on ${schema}.documents (sources_id);
create index documents_categories_id_idx on ${schema}.documents (categories_id);
create index documents_author_idx on ${schema}.documents (author);
create index documents_date_idx on ${schema}.documents (date);
create index documents_publication_idx on ${schema}.documents (publication);

create table ${schema}.word_ratios (
    documents_id        bigint  not null,
    word                varchar(100) not null,
    amount              integer not null,
    percent             real not null,
    rank                integer not null,
    clustering_score    float default 0 not null,
    constraint fk_documents foreign key (documents_id) references ${schema}.documents(id),
    constraint pk_files_id_word primary key (word, documents_id)
);

create index word_ratios_documents_id_idx on ${schema}.word_ratios (documents_id);

create table ${schema}.document_contents (
    documents_id    bigint not null,
    index           integer not null,
    subheading      varchar(500),
    content         text not null,
    constraint fk_documents foreign key (documents_id) references ${schema}.documents(id),
    constraint pk_document_contents primary key (documents_id, index)
);

create table ${schema}.similar_documents (
    main_document_id    bigint not null references ${schema}.documents(id),
    similar_document_id bigint not null references ${schema}.documents(id),
    similarity          float not null,
    constraint pk_similar_documents primary key (main_document_id, similar_document_id)
);

-- Create roles if they do not already exist. Looks convoluted due to the lack of a "create role if not exists" command.
do $do$
    begin
        if not exists (select from pg_catalog.pg_roles where rolname = 'readonly') then
            create role readonly NOINHERIT LOGIN password 'readonly';
        end if;
        if not exists (select from pg_catalog.pg_roles where rolname = 'read_write') then
            create role read_write NOINHERIT LOGIN password 'read_write';
        end if;
    end $do$;

revoke all privileges on all tables in schema ${schema} from readonly;
grant select on all tables in schema ${schema} to readonly;

revoke all privileges on all tables in schema ${schema} from read_write;
grant select, insert, update, delete on all tables in schema ${schema} to read_write;
