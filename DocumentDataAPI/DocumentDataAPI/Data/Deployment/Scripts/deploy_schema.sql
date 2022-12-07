-- Creates a copy of the document_data schema (see https://wiki.knox.cs.aau.dk/en/Database/DocumentDataAPI/document_data).
-- The actual name of the schema is provided though the ${schema} placeholder.
-- Also creates the readonly and read_write users that are used in production.
drop schema if exists ${schema} cascade;

create schema ${schema};

create table ${schema}.sources (
    id     bigint generated always as identity primary key,
    name   varchar(100) not null
);

create index sources_name_idx on ${schema}.sources (name);

create table ${schema}.categories (
    id     integer generated always as identity primary key,
    name   varchar(100) not null unique
);

create index categories_name_idx on ${schema}.categories (name);
insert into ${schema}.categories(name) values ('Uncategorized');

create table ${schema}.documents (
    id              bigint generated always as identity primary key,
    sources_id      bigint not null references ${schema}.sources(id),
    categories_id   bigint not null references ${schema}.categories(id),
    publication     varchar(100),
    title           varchar(400) not null,
    path            varchar(400) not null,
    summary         text,
    date            timestamp not null,
    author          varchar(200) not null,
    total_words     bigint not null,
    unique_words    bigint not null
);

create index documents_sources_id_idx on ${schema}.documents (sources_id);
create index documents_categories_id_idx on ${schema}.documents (categories_id);
create index documents_author_idx on ${schema}.documents (author);
create index documents_date_idx on ${schema}.documents (date);
create index documents_publication_idx on ${schema}.documents (publication);

create table ${schema}.word_ratios (
    documents_id        bigint not null references ${schema}.documents(id),
    word                varchar(100) not null,
    amount              integer not null,
    percent             real not null,
    rank                integer not null,
    clustering_score    float default 0 not null,
    tf_idf              float default 0 not null,
    constraint word_ratios_pkey primary key (word, documents_id)
);

create index word_ratios_documents_id_idx on ${schema}.word_ratios (documents_id);

create table ${schema}.document_contents (
    documents_id    bigint not null references ${schema}.documents(id),
    index           integer not null,
    subheading      text,
    content         text not null,
    constraint document_contents_pkey primary key (documents_id, index)
);

create table ${schema}.similar_documents (
    main_document_id    bigint not null references ${schema}.documents(id),
    similar_document_id bigint not null references ${schema}.documents(id),
    similarity          float not null,
    constraint similar_documents_pkey primary key (main_document_id, similar_document_id)
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

-- Grant privileges to these users, but first ensure that all privileges are revoked in case they already exist.
revoke all privileges on all tables in schema ${schema} from readonly, read_write;
grant usage on schema ${schema} to readonly, read_write;
grant usage on all sequences in schema ${schema} to readonly, read_write;
grant execute on all functions in schema ${schema} to readonly, read_write;
grant select on all tables in schema ${schema} to readonly, read_write;
grant temporary on database ${database} to readonly, read_write;

grant insert, update, delete on all tables in schema ${schema} to read_write;

-- Deploy schema for BiasDB

drop schema if exists 'BiasDB' CASCADE;

create schema 'BiasDB';

create table BiasDB.political_parties (
    id          int generated always as identity primary key,
    partyName   varchar(100) not null,
    partyBias   numeric[],
);

create index political_parties_partyName_idx on BiasDB.political_parties (partyName);

create table BiasDB.documents (
    id                      bigint generated always as identity primary key,
    party_id                int not null references BiasDB.political_parties(party_id),
    document                text,
    document_lemmatized     text
)

create index documents_party_id_idx on BiasDB.documents (party_id);

create table BiasDB.word_count (
    id                      bigint generated always as identity primary key,
    word                    VARCHAR(50) not NULL,
    count                   bigint not NULL
)

-- Uses the same readonly role and has a dedicated read/write role.
do $do$
    begin
        if not exists (select from pg_catalog.pg_roles where rolname = 'bias_read_write') then
            create role read_write NOINHERIT LOGIN password 'bias_read_write';
        end if;
    end $do$;

-- Grant privileges to these users, but first ensure that all privileges are revoked in case they already exist.
revoke all privileges on all tables in schema BiasDB from readonly, bias_read_write;
grant usage on schema BiasDB to readonly, bias_read_write;
grant usage on all sequences in schema BiasDB to readonly, bias_read_write;
grant execute on all functions in schema BiasDB to readonly, bias_read_write;
grant select on all tables in schema BiasDB to readonly, bias_read_write;
grant temporary on database ${database} to readonly, bias_read_write;

grant insert, update, delete on all tables in schema BiasDB to bias_read_write;