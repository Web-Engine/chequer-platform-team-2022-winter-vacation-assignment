grammar Sql;

root
    : statement END_OF_STATEMENT__?
;

statement
    : statementSelect
;

statementSelect
    :
        SELECT selectItems
        clauseFrom
        clauseWhere?
        clauseGroupBy?
        clauseOrderBy?
        clauseLimit?
;

selectItems
    : selectItem (COMMA__ selectItem)*
;
    
selectItem
    : referenceAllAttribute
//    Note: `expression` includes `referenceAttribute`
//    | referenceAttribute (AS? alias=identifier)?
    | expression (AS? alias=identifier)?
;

referenceAttribute
    : (relationIdentifier=identifier DOT__)? attributeIdentifier=identifier
;

referenceAllAttribute
    : (relationIdentifier=identifier DOT__)? ASTERISK__
;

clauseFrom
    : FROM fromItems 
;

fromItems
    : fromItem (COMMA__ fromItem)*
;

fromItem
    : relation (AS? alias=identifier) clauseJoin?
;

referenceRelation
    : relationIdentifier=identifier
;

relation
    : referenceRelation
    | OPEN_PARENTHESIS__ statementSelect CLOSE_PARENTHESIS__
;


clauseWhere
    : WHERE expression
;

clauseGroupBy
    : GROUP BY referenceAttribute clauseHaving?
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
    : LIMIT count=integer
    | LIMIT offset=integer COMMA__ count=integer
;

//region Expressions

expression
    : expression operator=AND expression                                            #expressionBinaryOperator
    | expression operator=OR expression                                             #expressionBinaryOperator
    | NOT expression                                                                #expressionNot
    | expressionValue operatorComparison expressionValue                            #expressionComparison
    | expressionValue                                                               #expressionValue_
;

expressionValue
    : OPEN_PARENTHESIS__ expressionInParenthesis CLOSE_PARENTHESIS__                 #expressionValueRequireParen
    | operatorUnary expressionValue                                                 #expressionValueUnary
    | expressionValue operator=(ASTERISK__ | SLASH__ | PERCENT__) expressionValue   #expressionValueBinaryOperator
    | expressionValue operator=(PLUS__ | MINUS__) expressionValue                   #expressionValueBinaryOperator
    | expressionFunctionCall                                                        #expressionValueFunctionCall
    | expressionPrimitive                                                           #expressionValuePrimitive
    | referenceAttribute                                                            #expressionValueReferenceAttribute
    ;

expressionInParenthesis
    : statementSelect
    | expression
;

expressionPrimitive
    : text
    | boolean
    | integer
    | double
;
   
expressionList
    : OPEN_PARENTHESIS__ expressionInParenthesis (COMMA__ expressionInParenthesis)* CLOSE_PARENTHESIS__
;

expressionFunctionCall
    : identifier expressionList 
;
//endregion Expressions

//region Commons
operatorComparison
    : EQ__  // '='
    | NEQ__ // '!='
    | GT__  // '>'
    | GTE__ // '>='
    | LT__  // '<'
    | LTE__ // '<='
;

operatorUnary
    : TILD__ // '~'
    | PLUS__
    | MINUS__
    | NOT
;

identifier
    : IDENTIFIER                                                #unquoted_identifier
    | DOUBLE_QUOTED_TEXT                                        #quoted_identifier
;

text
    : SINGLE_QUOTED_TEXT
;

integer
    : INTEGER
;

double
    : DOUBLE
;

boolean
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

EQ__: '=';
NEQ__: '!=';
GT__: '>';
GTE__: '>=';
LT__: '<';
LTE__: '<=';

PLUS__: '+';
MINUS__: '-';
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

ORDER: 'ORDER';
LIMIT: 'LIMIT';
NOT: 'NOT';

AND: 'AND';
OR: 'OR';
TRUE: 'TRUE';
FALSE: 'FALSE';

ASC: 'ASC';
DESC: 'DESC';

IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;

DOUBLE_QUOTED_TEXT: '"' (~[\r\n"] | '\\"')* '"';
SINGLE_QUOTED_TEXT: '\'' (~[\r\n'] | '\\\'')* '\'';

INTEGER: [+-]?[0-9]+;
DOUBLE: [+-]?[0-9]+ '.' [0-9]+;

END_OF_STATEMENT__: ';';

WHITESPACE: [ \r\n] -> skip;
