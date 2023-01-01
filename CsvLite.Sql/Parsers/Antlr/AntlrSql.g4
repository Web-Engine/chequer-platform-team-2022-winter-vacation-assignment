grammar AntlrSql;

options {
    caseInsensitive=true;
}

root
    : statement END_OF_STATEMENT__?
;

statement
    : statementSelect               #statement_select
    | statementInsert               #statement_insert
;

//region SELECT
statementSelect
    : SELECT
        selectItemList
        clauseFrom?
        clauseWhere?
        clauseGroupBy?
        clauseOrderBy?
        clauseLimit?
;

selectItemList
    : selectItem (COMMA__ selectItem)*
;
    
selectItem
    : referenceAllAttribute                                 #selectItem_all
    | selecItemSingle (AS? alias=identifier)?               #selectItem_single
;

selecItemSingle
    : referenceAttribute                                    #selectItemSingle_referenceAttribute
    | expression                                            #selectItemSingle_expression
    ;

clauseFrom
    : FROM fromItemList
;

fromItemList
    : fromItem (COMMA__ fromItem)*
;

fromItem
    : relation (AS? alias=identifier)? clauseJoin?
;

clauseWhere
    : WHERE expression
;

clauseGroupBy
    : GROUP BY referenceAttributeList clauseHaving?
;

clauseHaving
    : HAVING expression
;

clauseJoin
    : (LEFT | RIGHT)? (INNER | OUTER)? JOIN relation ON expression
;

clauseOrderBy
    : ORDER BY expression (ASC | DESC)?
;

clauseLimit
    : LIMIT count=literalInteger
    | LIMIT offset=literalInteger COMMA__ count=literalInteger
;
//endregion

//region INSERT
statementInsert
    : INSERT INTO
        insertItem
        insertValues
;

insertItem
    : relation attributeList?
;

attributeList
    : OPEN_PARENTHESIS__ attributeItem (COMMA__ attributeItem)* CLOSE_PARENTHESIS__
;

attributeItem
    : attributeIdentifier=identifier
;

insertValues
    : statementSelect                                       #insertValues_select
    | (VALUE | VALUES) valueList (COMMA__ valueList)*       #insertValues_values
;

valueList
    : OPEN_PARENTHESIS__ valueItem (COMMA__ valueItem)* CLOSE_PARENTHESIS__
;

valueItem
    : expression
;
//endregion

//region Expressions

expression
    : operator=NOT expression                                                                   #expression_unaryBooleanAlgebra
    | left=expression operator=AND right=expression                                             #expression_binaryBooleanAlgebra
    | left=expression operator=OR right=expression                                              #expression_binaryBooleanAlgebra
    | left=expressionValue
        operator=(
            EQ__ | NEQ__ | GT__ | GTE__ | LT__ | LTE__
        )
        right=expressionValue                                                                   #expression_comparison
    | expressionValue                                                                           #expression_value
;

expressionValue
    : OPEN_PARENTHESIS__ expressionInParenthesis CLOSE_PARENTHESIS__                            #expressionValue_parenthesis
    | operator=(TILD__ | PLUS__ | DASH__) expressionValue                                       #expressionValue_unary
    | left=expressionValue operator=(ASTERISK__ | SLASH__ | PERCENT__) right=expressionValue    #expressionValue_binary
    | left=expressionValue operator=(PLUS__ | DASH__) right=expressionValue                     #expressionValue_binary
    | expressionFunctionCall                                                                    #expressionValue_functionCall
    | expressionLiteral                                                                         #expressionValue_literal
    | referenceAttribute                                                                        #expressionValue_referenceAttribute
    ;

expressionInParenthesis
    : statementSelect                   #expressionInParenthesis_select
    | expression                        #expressionInParenthesis_expression
;

expressionLiteral
    : literalString                     #expressionLiteral_string
    | literalBoolean                    #expressionLiteral_boolean
    | literalInteger                    #expressionLiteral_integer
    | literalDouble                     #expressionLiteral_double
;
   
expressionList
    : expression (COMMA__ expression)*
;

expressionFunctionCall
    : COUNT OPEN_PARENTHESIS__ ASTERISK__ CLOSE_PARENTHESIS__             #expressionFunctionCall_count
    | COUNT OPEN_PARENTHESIS__ expression CLOSE_PARENTHESIS__             #expressionFunctionCall_count
    | identifier OPEN_PARENTHESIS__ expressionList CLOSE_PARENTHESIS__    #expressionFunctionCall_function
;
//endregion Expressions

//region Commons
referenceRelation
    : relationIdentifier=identifier
;

referenceAttributeList
    : referenceAttribute (COMMA__ referenceAttribute)*
;

referenceAttribute
    : (relationIdentifier=identifier DOT__)? attributeIdentifier=identifier
;

referenceAllAttribute
    : (relationIdentifier=identifier DOT__)? ASTERISK__
;

relation
    : referenceRelation                                         #relation_reference
    | OPEN_PARENTHESIS__ statementSelect CLOSE_PARENTHESIS__    #relation_select
;

identifier
    : IDENTIFIER                                                #identifier_unquoted
    | DOUBLE_QUOTED_TEXT                                        #identifier_quoted
;

literalString
    : SINGLE_QUOTED_TEXT
;

literalInteger
    : INTEGER
;

literalDouble
    : DOUBLE
;

literalBoolean
    : TRUE
    | FALSE
;
//endregion Commons

DOT__: '.';
ASTERISK__: '*';
COMMA__: ',';
OPEN_PARENTHESIS__: '(';
CLOSE_PARENTHESIS__: ')';

TILD__: '~';

GTE__: '>=';
LTE__: '<=';
NEQ__: '!=';
EQ__: '=';
GT__: '>';
LT__: '<';

PLUS__: '+';
DASH__: '-';
SLASH__: '/';
PERCENT__: '%';


SELECT: 'SELECT';
FROM: 'FROM';
AS: 'AS';
WHERE: 'WHERE';
GROUP: 'GROUP';
BY: 'BY';
HAVING: 'HAVING';
JOIN: 'JOIN';
LEFT: 'LEFT';
RIGHT: 'RIGHT';
INNER: 'INNER';
OUTER: 'OUTER';
ON: 'ON';
COUNT: 'COUNT';

ORDER: 'ORDER';
LIMIT: 'LIMIT';

INSERT: 'INSERT';
INTO: 'INTO';
VALUE: 'VALUE';
VALUES: 'VALUES';

NOT: 'NOT';

AND: 'AND';
OR: 'OR';
TRUE: 'TRUE';
FALSE: 'FALSE';

ASC: 'ASC';
DESC: 'DESC';

IDENTIFIER: [a-z_][a-z0-9_]*;

DOUBLE: [0-9]+ '.' [0-9]+;
INTEGER: [0-9]+;

DOUBLE_QUOTED_TEXT: '"' (~[\r\n"] | '\\"')* '"';
SINGLE_QUOTED_TEXT: '\'' (~[\r\n'] | '\\\'')* '\'';

END_OF_STATEMENT__: ';';

WHITESPACE: [ \r\n] -> skip;
