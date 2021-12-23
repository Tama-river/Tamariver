DROP TABLE IF EXISTS sky;
CREATE TABLE sky (
mid text,
r integer,
y integer,
g integer,
b integer,
p integer,
w integer,
d integer
);

DROP TABLE IF EXISTS word;
CREATE TABLE word (
feel text,
r integer,
y integer,
g integer,
b integer,
p integer,
w integer,
d integer
);

INSERT INTO word (feel, r, y, g, b, p, w, d)
VALUES ('warm', 14, 13, -1, -13, -7, -4, 1);
INSERT INTO word (feel, r, y, g, b, p, w, d)
VALUES ('bright', 11, 22, 6, -6, -9, 14, -22);
INSERT INTO word (feel, r, y, g, b, p, w, d)
VALUES ('light', 0, 16, 3, -4, -12, 11, -19);
INSERT INTO word (feel, r, y, g, b, p, w, d)
VALUES ('beautiful', 5, 6, 4, 1, -3, 14, 4);
INSERT INTO word (feel, r, y, g, b, p, w, d)
VALUES ('vivid', 8, 17, 7, 6, -2, 9, 12);
INSERT INTO word (feel, r, y, g, b, p, w, d)
VALUES ('clear', 1, 12, 4, 3, -6, 15, -1);
INSERT INTO word (feel, r, y, g, b, p, w, d)
VALUES ('dark', -11, -22, -6, 6, 9, -14, 22);
INSERT INTO word (feel, r, y, g, b, p, w, d)
VALUES ('emotionnal', 14, 8, -4, -6, 1, -11, -7);
INSERT INTO word (feel, r, y, g, b, p, w, d)
VALUES ('grave', 0, -16, -3, 4, 12, -11, 19);
INSERT INTO word (feel, r, y, g, b, p, w, d)
VALUES ('dynamic', 12, 13, 2, -6, -4, -11, -9);
INSERT INTO word (feel, r, y, g, b, p, w, d)
VALUES ('gloomy', -11, -16, 1, 6, 9, -4, 13);
INSERT INTO word (feel, r, y, g, b, p, w, d)
VALUES ('vagne', -8, -17, -7, -6, 2, -9, -12);

INSERT INTO sky (mid, r, y, g, b, p, w, d)
VALUES ('https://imageslabo.com/wp-content/uploads/2019/05/353_sky_cloud_6788.jpg', -10, -5, 0, 14, 2, 7, -20);
INSERT INTO sky (mid, r, y, g, b, p, w, d)
VALUES ('https://capa.getnavi.jp/wps/wp-content/uploads/2018/11/181122habu.jpg', 3, -9, 0, 11, 19, 6, 7);
INSERT INTO sky (mid, r, y, g, b, p, w, d)
VALUES ('https://pds.exblog.jp/pds/1/201107/30/79/f0121379_2113431.jpg', 10, 8, 0, -8, 1, 0, 8);
INSERT INTO sky (mid, r, y, g, b, p, w, d)
VALUES ('https://www.helpforest.com/break/sora/img/sora0306.jpg', -14, -11, 2, 12, 7, -4, 10);
INSERT INTO sky (mid, r, y, g, b, p, w, d)
VALUES ('https://cdn-ak.f.st-hatena.com/images/fotolife/n/narumi087/20190422/20190422204847.jpg', 7, 3, 1, 11, 1, 5, -10);
INSERT INTO sky (mid, r, y, g, b, p, w, d)
VALUES ('https://mitoune.c.blog.ss-blog.jp/_images/blog/_691/mitoune/20210508R-1.jpg', -17, -1, -13, -8, -16, 20, 3);
INSERT INTO sky (mid, r, y, g, b, p, w, d)
VALUES ('https://kobaxk2.up.seesaa.net/ftp/cloud01.jpg', -22, -17, -20, -15, -13, 3, 13);
INSERT INTO sky (mid, r, y, g, b, p, w, d)
VALUES ('https://pbs.twimg.com/media/Dgc5IzqVAAAGHJu?format=jpg', 6, -18, -21, 7, 19, -10, 4);
INSERT INTO sky (mid, r, y, g, b, p, w, d)
VALUES ('https://ikinariblog.com/wp-content/uploads/2017/11/040_convert_20120118131739.jpg', 22, 7, -18, 1, 9, -14, 17);
INSERT INTO sky (mid, r, y, g, b, p, w, d)
VALUES ('https://p1.pxfuel.com/preview/896/1009/194/green-water-ocean-sea.jpg', -17, 8, 21, 3, 0, 1, 11);

DROP FUNCTION IF EXISTS inner_product(int4, int4, int4, int4, int4, int4, int4, int4, int4, int4, int4, int4, int4, int4);
CREATE FUNCTION inner_product(int4, int4, int4, int4, int4, int4, int4, int4, int4, int4, int4, int4, int4, int4)
RETURNS int4 AS
'SELECT $1 * $8 + $2 * $9 + $3 * $10 + $4 * $11 + $5 * $12 + $6 * $13 + $7 * $14 AS inner_product;'
LANGUAGE 'sql';

SELECT sky.mid, inner_product(sky.r, sky.y, sky.g, sky.b, sky.p, sky.w, sky.d, word.r, word.y, word.g, word.b, word.p, word.w, word.d)
AS score
FROM sky, word WHERE word.feel='dark'
ORDER BY score DESC;