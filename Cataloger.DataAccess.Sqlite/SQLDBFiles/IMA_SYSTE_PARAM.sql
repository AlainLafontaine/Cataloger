-- 1. Désactiver les contraintes de clé étrangère temporairement
PRAGMA foreign_keys=OFF;

-- 2. Démarrer une transaction
BEGIN TRANSACTION;

-- 3. Renommer l'ancienne table
ALTER TABLE CAT_SYSTE_PARAM RENAME TO CAT_SYSTE_PARAM_OLD;

-- 4. Créer la nouvelle table avec AUTOINCREMENT
CREATE TABLE [CAT_SYSTE_PARAM](
  [SPM_NO_SEQ] INTEGER PRIMARY KEY NOT NULL UNIQUE, 
  [SPM_SECTION] VARCHAR(32) NOT NULL, 
  [SPM_KEY] VARCHAR(32) NOT NULL, 
  [SPM_DESCRIPTION] VARCHAR(128) NOT NULL, 
  [SPM_VAL_STR] VARCHAR(256), 
  [SPM_VAL_LONG] INTEGER, 
  [SPM_VAL_DOUBLE] DOUBLE, 
  [SPM_VAL_DATE] DATE, 
  [SPM_VAL_BOOL] BOOLEAN, 
  [SPM_VAL_CHAR] VARCHAR(1), 
  UNIQUE([SPM_SECTION], [SPM_KEY])
) WITHOUT ROWID;

-- 5. Copier les données
INSERT INTO CAT_SYSTE_PARAM SELECT * FROM CAT_SYSTE_PARAM_OLD;

-- 6. Supprimer l'ancienne table
DROP TABLE CAT_SYSTE_PARAM_OLD;

-- 7. Valider la transaction
COMMIT;

-- 8. Réactiver les contraintes
PRAGMA foreign_keys=ON;


