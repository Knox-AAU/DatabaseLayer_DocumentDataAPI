-- Deploy schema for Bias_data

drop schema if exists ${schema} CASCADE;

create schema ${schema};

create table ${schema}.political_parties (
    id          int generated always as identity primary key,
    partyName   varchar(100) not null,
    partyBias   numeric[]
);

create index political_parties_partyName_idx on ${schema}.political_parties (partyName);

create table ${schema}.documents (
    id                      bigint generated always as identity primary key,
    party_id                int not null references ${schema}.political_parties(id),
    document                text,
    document_lemmatized     text,
    url                     text
);

create index documents_party_id_idx on ${schema}.documents (party_id);

create table ${schema}.word_count (
    id                      bigint generated always as identity primary key,
    word                    VARCHAR(50) not NULL,
    count                   bigint not NULL
);

create index word_count_word_idx on ${schema}.word_count (word);

-- Uses the same readonly role and has a dedicated read/write role.
do $do$
    begin
        if not exists (select from pg_catalog.pg_roles where rolname = 'bias_read_write') then
            create role bias_read_write NOINHERIT LOGIN password 'bias_read_write';
        end if;
    end $do$;

-- Grant privileges to these users, but first ensure that all privileges are revoked in case they already exist.
revoke all privileges on all tables in schema ${schema} from readonly, bias_read_write;
grant usage on schema ${schema} to readonly, bias_read_write;
grant usage on all sequences in schema ${schema} to readonly, bias_read_write;
grant execute on all functions in schema ${schema} to readonly, bias_read_write;
grant select on all tables in schema ${schema} to readonly, bias_read_write;
grant temporary on database ${database} to readonly, bias_read_write;

grant insert, update, delete on all tables in schema ${schema} to bias_read_write;