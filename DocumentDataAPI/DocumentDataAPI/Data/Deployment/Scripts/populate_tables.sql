-- Populates the given document_data schema with test data.

INSERT INTO ${schema}.data_sources ("name") VALUES
    ('DR'),
    ('TV2');

INSERT INTO ${schema}.categories (name) VALUES
    ('Nyhedsartikel');

INSERT INTO ${schema}.documents (id,sources_id,title,"path",summary,"date",author,total_words, categories_id, unique_words) VALUES
    (1,1,'Iran hævder, at Mahsa Amini døde af organsvigt','https://www.dr.dk/nyheder/seneste/iran-haevder-mahsa-amini-doede-af-organsvigt','','2022-10-07 13:40:00','Maja Lærke Maach',0, 1, 0),
    (2,1,'Kongehuset: Dronningen har talt med prins Joachim på Fredensborg Slot','https://www.dr.dk/nyheder/seneste/kongehuset-dronningen-har-talt-med-prins-joachim-paa-fredensborg-slot','','2022-10-07 13:33:00','Maja Lærke Maach',0, 1, 0),
    (3,1,'Radikale vil have nationalt kompromis om unges trivsel','https://www.dr.dk/nyheder/seneste/radikale-vil-have-nationalt-kompromis-om-unges-trivsel','','2022-10-06 23:55:00','Andreas Nygaard Just',0, 1, 0),
    (4,2,'Eks-landsholdsatlet skal i fængsel for grov vold og voldtægt','https://nyheder.tv2.dk/live/2022-10-07-nyhedsoverblik#entry=3830920','','2022-10-07 05:01:00','Mette Stender Pedersen',0, 1, 0),
    (5,2,'Folk stod i timelange køer for vacciner','https://nyheder.tv2.dk/lokalt/2022-10-03-folk-stod-i-timelange-koeer-for-vacciner','','2022-10-03 09:01:00','Jonathan Kjær Troelsen',0, 1, 0);

INSERT INTO ${schema}.document_contents (documents_id,"content",index) VALUES
    (1,'Den unge kvinde Mahsa Amini, som døde efter at være blevet banket af Irans moralpoliti, afgik ikke ved døden som følge af slag mod hovedet, arme og ben.
Hun døde af organsvigt, som følge af, at hun ikke fik nok ilt til hjernen. Det påstår det iranske styre i en rapport fra iranske retsmedicinere.
Menneskerettighedsorganisationer er dog af en anden overbevisning og mener, at kvinden døde af sine kvæstelser efter at være blevet tilbageholdt for ikke at dække sit hår til med et tørklæde.
Mahsa Aminis død i moralpolitiets varetægt udløste verdensomspændende demonstrationer mod styret i landet.
', 0),
    (2,'Dronning Margrethe og prins Joachim har talt sammen på Fredensborg Slot.
Det bekræfter kongehusets kommunikationschef, Lene Balleby, overfor B.T.
Det er angiveligt første gang, at de to mødes efter nyheden om, at dronningen fra næste år fratager prinsens børn deres titler.
En nyhed som chokerede prinsen og hans del af den kongelige familie i en sådan grad, at han gik ud i pressen og åbent kritiserede beslutningen og ikke mindst hele forløbet.
Ifølge hans eget udsagn fik han fem dage til at forberede sine børn på den nye virkelighed.
- Alle er enige om at se fremad, og som dronningen selv har givet udtryk for, så ønsker hun og prins Joachim ro til at finde vej igennem denne situation, siger Lene Balleby til B.T.
', 0),
    (3,'Under torsdagens åbningsdebat i Folketinget, der sluttede for kort tid siden, faldt snakken blandt andet på den mentale trivsel blandt landets børn og unge.
Radikale Venstre håber, at der efter valget kan blive lavet et nationalt kompromis for at løse problemerne for børn og unge og deres trivsel – ligesom der er lavet et nationalt kompromis om oprustning af forsvaret efter Ruslands invasion af Ukraine.
For ellers tror den radikale ordfører, Lotte Rod, ikke, at der sker noget.
- De er blevet syge af at gå i institution og skole, fordi de voksne ikke har tid til dem.
- Derfor vil jeg gerne spørge statsministeren, om vi ikke også skal lave et nationalt kompromis om børn og unges trivsel?, spurgte Lotte Rod.
Til det svarede statsministeren:
Der er i hvert fald behov for at arbejde sammen på det her område os, sagde Mette Frederiksen.
Du kan se, hvordan debatten udviklede sig, her.
    ', 0),
    (4,'Den tidligere landsholdstrampolinspringer Daniel Præst er fredag idømt seks års fængsel i en sag om særlig farlig voldtægt, grov vold, røveri og trusler. Dommen er faldet ved Retten i Helsingør. Ved voldtægten mistede offeret bevidstheden og var i livsfare.', 0),
    (5,'Det er blevet muligt for personer over 50 år at få deres fjerde vaccinestik mod corona. Men vaccinationerne er ikke forløbet problemfrit.
Hvis man er blandt de personer, som de seneste dage har været ude og blive vaccineret, så er der en vis chance for, at man har stået i en lang kø.
Fra weekenden af blev det muligt for personer over 50 år at få sit fjerde vaccinationsstik, men hos nogle af vaccinationsstederne har de ikke kunne følge med tempoet.
- Der har været nogle forskellige udfordringer med afviklingen på et par af vaccinationscentrene i weekenden, hvilket beklageligvis har resulteret i ekstra ventetid, siger Anders Cinicola, chefkonsulent ved Sundhedsplanlægning i Region Nordjylland.
Det er blandt andet i Frederikshavn, at der har været timelange ventekøer, men der bliver arbejdet på, at der i de kommende dage ikke opstår ventetid.
- Vi følger op på, hvad der præcist er gået galt og vil bestræbe os på, at de fremtidige vaccinationsdage forløber bedre. Vi har naturligvis en ambition om, at borgerne skal opleve et effektivt vaccinationstilbud og kortest mulig ventetid, siger Anders Cinicola.
Regionen har sendt vaccineinvitationer gennem digital post til omkring 250.000 nordjyder.
', 0);

INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (1,'den',1,1.03,0),
    (1,'unge',1,1.03,0),
    (1,'kvinde',1,1.03,0),
    (1,'mahsa',2,2.06,0),
    (1,'som',3,3.09,0),
    (1,'døde',3,3.09,0),
    (1,'efter',2,2.06,0),
    (1,'være',2,2.06,0),
    (1,'blevet',2,2.06,0),
    (1,'banket',1,1.03,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (1,'irans',1,1.03,0),
    (1,'moralpoliti',1,1.03,0),
    (1,'afgik',1,1.03,0),
    (1,'ikke',3,3.09,0),
    (1,'ved',1,1.03,0),
    (1,'døden',1,1.03,0),
    (1,'folge',2,2.06,0),
    (1,'slag',1,1.03,0),
    (1,'mod',2,2.06,0),
    (1,'hovedet',1,1.03,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (1,'arme',1,1.03,0),
    (1,'og',2,2.06,0),
    (1,'ben',1,1.03,0),
    (1,'hun',2,2.06,0),
    (1,'fik',1,1.03,0),
    (1,'nok',1,1.03,0),
    (1,'ilt',1,1.03,0),
    (1,'til',2,2.06,0),
    (1,'hjernen',1,1.03,0),
    (1,'det',2,2.06,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (1,'påstår',1,1.03,0),
    (1,'iranske',2,2.06,0),
    (1,'styre',1,1.03,0),
    (1,'i',3,3.09,0),
    (1,'en',2,2.06,0),
    (1,'rapport',1,1.03,0),
    (1,'fra',1,1.03,0),
    (1,'retsmedicinere',1,1.03,0),
    (1,'menneskerettighedsorganisationer',1,1.03,0),
    (1,'er',1,1.03,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (1,'dog',1,1.03,0),
    (1,'anden',1,1.03,0),
    (1,'overbevisning',1,1.03,0),
    (1,'mener',1,1.03,0),
    (1,'kvinden',1,1.03,0),
    (1,'sine',1,1.03,0),
    (1,'kvæstelser',1,1.03,0),
    (1,'tilbageholdt',1,1.03,0),
    (1,'for',1,1.03,0),
    (1,'dække',1,1.03,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (1,'sit',1,1.03,0),
    (1,'hår',1,1.03,0),
    (1,'med',1,1.03,0),
    (1,'et',1,1.03,0),
    (1,'torklæde',1,1.03,0),
    (1,'aminis',1,1.03,0),
    (1,'dod',1,1.03,0),
    (1,'moralpolitiets',1,1.03,0),
    (1,'varetægt',1,1.03,0),
    (1,'udloste',1,1.03,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (1,'verdensomspændende',1,1.03,0),
    (1,'demonstrationer',1,1.03,0),
    (1,'styret',1,1.03,0),
    (1,'landet',1,1.03,0),
    (2,'dronning',1,0.81,0),
    (2,'margrethe',1,0.81,0),
    (2,'og',6,4.84,0),
    (2,'sammen',1,0.81,0),
    (2,'det',2,1.61,0),
    (2,'bekræfter',1,0.81,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (2,'kongehusets',1,0.81,0),
    (2,'kommunikationschef',1,0.81,0),
    (2,'lene',2,1.61,0),
    (2,'balleby',2,1.61,0),
    (2,'overfor',1,0.81,0),
    (2,'bt',2,1.61,0),
    (2,'er',2,1.61,0),
    (2,'angiveligt',1,0.81,0),
    (2,'forste',1,0.81,0),
    (2,'gang',1,0.81,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (2,'at',6,4.84,0),
    (2,'de',1,0.81,0),
    (2,'to',1,0.81,0),
    (2,'modes',1,0.81,0),
    (2,'efter',1,0.81,0),
    (2,'nyheden',1,0.81,0),
    (2,'om',2,1.61,0),
    (2,'fra',1,0.81,0),
    (2,'næste',1,0.81,0),
    (2,'år',1,0.81,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (2,'fratager',1,0.81,0),
    (2,'prinsens',1,0.81,0),
    (2,'born',2,1.61,0),
    (2,'deres',1,0.81,0),
    (2,'titler',1,0.81,0),
    (2,'en',2,1.61,0),
    (2,'nyhed',1,0.81,0),
    (2,'som',2,1.61,0),
    (2,'chokerede',1,0.81,0),
    (2,'prinsen',1,0.81,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (2,'hans',2,1.61,0),
    (2,'del',1,0.81,0),
    (2,'af',1,0.81,0),
    (2,'den',2,1.61,0),
    (2,'kongelige',1,0.81,0),
    (2,'familie',1,0.81,0),
    (2,'i',2,1.61,0),
    (2,'sådan',1,0.81,0),
    (2,'grad',1,0.81,0),
    (2,'han',2,1.61,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (2,'gik',1,0.81,0),
    (2,'ud',1,0.81,0),
    (2,'pressen',1,0.81,0),
    (2,'åbent',1,0.81,0),
    (2,'kritiserede',1,0.81,0),
    (2,'beslutningen',1,0.81,0),
    (2,'ikke',1,0.81,0),
    (2,'mindst',1,0.81,0),
    (2,'hele',1,0.81,0),
    (2,'forlobet',1,0.81,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (2,'ifolge',1,0.81,0),
    (2,'eget',1,0.81,0),
    (2,'udsagn',1,0.81,0),
    (2,'fik',1,0.81,0),
    (2,'fem',1,0.81,0),
    (2,'dage',1,0.81,0),
    (2,'til',3,2.42,0),
    (2,'forberede',1,0.81,0),
    (2,'sine',1,0.81,0),
    (2,'prins',2,1.61,1);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (2,'joachim',2,1.61,1),
    (2,'har',2,1.61,1),
    (2,'nye',1,0.81,0),
    (2,'virkelighed',1,0.81,0),
    (2,'alle',1,0.81,0),
    (2,'enige',1,0.81,0),
    (2,'se',1,0.81,0),
    (2,'fremad',1,0.81,0),
    (2,'selv',1,0.81,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (2,'givet',1,0.81,0),
    (2,'udtryk',1,0.81,0),
    (2,'for',1,0.81,0),
    (2,'så',1,0.81,0),
    (2,'onsker',1,0.81,0),
    (2,'hun',1,0.81,0),
    (2,'ro',1,0.81,0),
    (2,'finde',1,0.81,0),
    (2,'vej',1,0.81,0),
    (2,'igennem',1,0.81,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (2,'denne',1,0.81,0),
    (2,'situation',1,0.81,0),
    (2,'siger',1,0.81,0),
    (3,'under',1,0.65,0),
    (3,'torsdagens',1,0.65,0),
    (3,'åbningsdebat',1,0.65,0),
    (3,'i',3,1.96,0),
    (3,'folketinget',1,0.65,0),
    (3,'der',5,3.27,0),
    (3,'sluttede',1,0.65,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (3,'for',5,3.27,0),
    (3,'kort',1,0.65,0),
    (3,'tid',2,1.31,0),
    (3,'siden',1,0.65,0),
    (3,'faldt',1,0.65,0),
    (3,'snakken',1,0.65,0),
    (3,'blandt',2,1.31,0),
    (3,'andet',1,0.65,0),
    (3,'på',2,1.31,0),
    (3,'den',2,1.31,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (3,'mentale',1,0.65,0),
    (3,'landets',1,0.65,0),
    (3,'born',3,1.96,0),
    (3,'og',5,3.27,0),
    (3,'unge',2,1.31,0),
    (3,'venstre',1,0.65,0),
    (3,'håber',1,0.65,0),
    (3,'at',5,3.27,0),
    (3,'efter',2,1.31,0),
    (3,'valget',1,0.65,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (3,'kan',2,1.31,0),
    (3,'blive',1,0.65,0),
    (3,'lavet',2,1.31,0),
    (3,'et',3,1.96,0),
    (3,'lose',1,0.65,0),
    (3,'problemerne',1,0.65,0),
    (3,'deres',1,0.65,0),
    (3,'ligesom',1,0.65,0),
    (3,'er',3,1.96,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (3,'oprustning',1,0.65,0),
    (3,'af',3,1.96,0),
    (3,'forsvaret',1,0.65,0),
    (3,'ruslands',1,0.65,0),
    (3,'invasion',1,0.65,0),
    (3,'ukraine',1,0.65,0),
    (3,'ellers',1,0.65,0),
    (3,'tror',1,0.65,0),
    (3,'ordforer',1,0.65,0),
    (3,'lotte',2,1.31,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (3,'rod',2,1.31,0),
    (3,'ikke',3,1.96,0),
    (3,'sker',1,0.65,0),
    (3,'noget',1,0.65,0),
    (3,'de',2,1.31,0),
    (3,'blevet',1,0.65,0),
    (3,'syge',1,0.65,0),
    (3,'gå',1,0.65,0),
    (3,'institution',1,0.65,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (3,'skole',1,0.65,0),
    (3,'fordi',1,0.65,0),
    (3,'voksne',1,0.65,0),
    (3,'har',1,0.65,0),
    (3,'til',2,1.31,0),
    (3,'dem',1,0.65,0),
    (3,'derfor',1,0.65,0),
    (3,'jeg',1,0.65,0),
    (3,'gerne',1,0.65,0),
    (3,'sporge',1,0.65,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (3,'statsministeren',2,1.31,0),
    (3,'vi',1,0.65,0),
    (3,'også',1,0.65,0),
    (3,'skal',1,0.65,0),
    (3,'lave',1,0.65,0),
    (3,'trivsel?',1,0.65,0),
    (3,'spurgte',1,0.65,0),
    (3,'det',2,1.31,0),
    (3,'svarede',1,0.65,0),
    (3,'hvert',1,0.65,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (3,'fald',1,0.65,0),
    (3,'behov',1,0.65,0),
    (3,'arbejde',1,0.65,0),
    (3,'sammen',1,0.65,0),
    (3,'her',2,1.31,0),
    (3,'område',1,0.65,0),
    (3,'os',1,0.65,0),
    (3,'sagde',1,0.65,0),
    (3,'mette',1,0.65,0),
    (3,'frederiksen',1,0.65,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (3,'du',1,0.65,0),
    (3,'se',1,0.65,0),
    (3,'hvordan',1,0.65,0),
    (3,'debatten',1,0.65,0),
    (3,'udviklede',1,0.65,0),
    (3,'sig',1,0.65,0),
    (4,'den',1,2.56,0),
    (4,'tidligere',1,2.56,0),
    (4,'landsholdstrampolinspringer',1,2.56,0),
    (4,'daniel',1,2.56,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (4,'præst',1,2.56,0),
    (4,'er',2,5.13,0),
    (4,'fredag',1,2.56,0),
    (4,'idomt',1,2.56,0),
    (4,'seks',1,2.56,0),
    (4,'års',1,2.56,0),
    (4,'en',1,2.56,0),
    (4,'sag',1,2.56,0),
    (4,'om',1,2.56,0),
    (4,'særlig',1,2.56,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (4,'farlig',1,2.56,0),
    (4,'fængsel',1,2.56,1),
    (4,'i',3,7.69,1),
    (4,'voldtægt',1,2.56,1),
    (4,'grov',1,2.56,1),
    (4,'vold',1,2.56,1),
    (4,'roveri',1,2.56,0),
    (4,'trusler',1,2.56,0),
    (4,'dommen',1,2.56,0),
    (4,'faldet',1,2.56,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (4,'ved',2,5.13,0),
    (4,'retten',1,2.56,0),
    (4,'helsingor',1,2.56,0),
    (4,'voldtægten',1,2.56,0),
    (4,'mistede',1,2.56,0),
    (4,'offeret',1,2.56,0),
    (4,'bevidstheden',1,2.56,0),
    (4,'var',1,2.56,0),
    (4,'livsfare',1,2.56,0),
    (5,'det',3,1.56,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (5,'er',6,3.12,0),
    (5,'blevet',1,0.52,0),
    (5,'muligt',2,1.04,0),
    (5,'personer',3,1.56,0),
    (5,'over',2,1.04,0),
    (5,'50',2,1.04,0),
    (5,'år',2,1.04,0),
    (5,'at',7,3.65,0),
    (5,'få',2,1.04,0),
    (5,'deres',1,0.52,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (5,'fjerde',2,1.04,0),
    (5,'vaccinestik',1,0.52,0),
    (5,'mod',1,0.52,0),
    (5,'corona',1,0.52,0),
    (5,'men',3,1.56,0),
    (5,'vaccinationerne',1,0.52,0),
    (5,'ikke',3,1.56,0),
    (5,'forlobet',1,0.52,0),
    (5,'problemfrit',1,0.52,0),
    (5,'hvis',1,0.52,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (5,'man',2,1.04,0),
    (5,'blandt',2,1.04,0),
    (5,'de',5,2.6,0),
    (5,'som',1,0.52,0),
    (5,'seneste',1,0.52,0),
    (5,'dage',2,1.04,0),
    (5,'har',8,4.17,0),
    (5,'været',3,1.56,0),
    (5,'ude',1,0.52,0),
    (5,'og',3,1.56,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (5,'blive',1,0.52,0),
    (5,'vaccineret',1,0.52,0),
    (5,'så',1,0.52,0),
    (5,'der',6,3.12,0),
    (5,'en',3,1.56,0),
    (5,'vis',1,0.52,0),
    (5,'chance',1,0.52,0),
    (5,'stået',1,0.52,0),
    (5,'lang',1,0.52,0),
    (5,'ko',1,0.52,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (5,'fra',1,0.52,0),
    (5,'weekenden',2,1.04,0),
    (5,'af',3,1.56,0),
    (5,'blev',1,0.52,0),
    (5,'sit',1,0.52,0),
    (5,'vaccinationsstik',1,0.52,0),
    (5,'hos',1,0.52,0),
    (5,'nogle',2,1.04,0),
    (5,'vaccinationsstederne',1,0.52,0),
    (5,'kunne',1,0.52,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (5,'folge',1,0.52,0),
    (5,'med',2,1.04,0),
    (5,'tempoet',1,0.52,0),
    (5,'forskellige',1,0.52,0),
    (5,'udfordringer',1,0.52,0),
    (5,'afviklingen',1,0.52,0),
    (5,'på',4,2.08,0),
    (5,'et',2,1.04,0),
    (5,'par',1,0.52,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (5,'vaccinationscentrene',1,0.52,0),
    (5,'hvilket',1,0.52,0),
    (5,'beklageligvis',1,0.52,0),
    (5,'resulteret',1,0.52,0),
    (5,'ekstra',1,0.52,0),
    (5,'ventetid',3,1.56,0),
    (5,'siger',2,1.04,0),
    (5,'anders',2,1.04,0),
    (5,'cinicola',2,1.04,0),
    (5,'chefkonsulent',1,0.52,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (5,'ved',1,0.52,0),
    (5,'sundhedsplanlægning',1,0.52,0),
    (5,'region',1,0.52,0),
    (5,'nordjylland',1,0.52,0),
    (5,'andet',1,0.52,0),
    (5,'frederikshavn',1,0.52,0),
    (5,'ventekoer',1,0.52,0),
    (5,'bliver',1,0.52,0),
    (5,'arbejdet',1,0.52,0),
    (5,'kommende',1,0.52,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (5,'opstår',1,0.52,0),
    (5,'vi',2,1.04,0),
    (5,'folger',1,0.52,0),
    (5,'op',1,0.52,0),
    (5,'hvad',1,0.52,0),
    (5,'præcist',1,0.52,0),
    (5,'gået',1,0.52,0),
    (5,'galt',1,0.52,0),
    (5,'vil',1,0.52,0),
    (5,'bestræbe',1,0.52,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (5,'os',1,0.52,0),
    (5,'fremtidige',1,0.52,0),
    (5,'vaccinationsdage',1,0.52,0),
    (5,'forlober',1,0.52,0),
    (5,'bedre',1,0.52,0),
    (5,'naturligvis',1,0.52,0),
    (5,'ambition',1,0.52,0),
    (5,'om',1,0.52,0),
    (5,'borgerne',1,0.52,0),
    (5,'skal',1,0.52,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (5,'opleve',1,0.52,0),
    (5,'effektivt',1,0.52,0),
    (5,'vaccinationstilbud',1,0.52,0),
    (5,'kortest',1,0.52,0),
    (5,'mulig',1,0.52,0),
    (5,'regionen',1,0.52,0),
    (5,'sendt',1,0.52,0),
    (5,'vaccineinvitationer',1,0.52,0),
    (5,'gennem',1,0.52,0),
    (5,'digital',1,0.52,0);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (5,'post',1,0.52,0),
    (5,'til',1,0.52,0),
    (5,'omkring',1,0.52,0),
    (5,'250000',1,0.52,0),
    (1,'amini',1,1.03,1),
    (1,'at',5,5.15,1),
    (1,'af',6,6.19,1),
    (1,'organsvigt',1,1.03,1),
    (2,'talt',1,0.81,1),
    (2,'på',2,1.61,1);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (2,'fredensborg',1,0.81,1),
    (5,'for',3,1.56,1),
    (2,'slot',1,0.81,1),
    (2,'dronningen',2,1.61,1),
    (3,'trivsel',2,1.31,1),
    (3,'radikale',2,1.31,1),
    (3,'nationalt',3,1.96,1),
    (3,'kompromis',3,1.96,1),
    (3,'om',3,1.96,1),
    (3,'vil',1,0.65,1);
INSERT INTO ${schema}.word_ratios (documents_id,word,amount,"percent","rank") VALUES
    (3,'unges',1,0.65,1),
    (4,'og',2,5.13,1),
    (5,'i',6,3.12,1),
    (5,'timelange',1,0.52,1);
