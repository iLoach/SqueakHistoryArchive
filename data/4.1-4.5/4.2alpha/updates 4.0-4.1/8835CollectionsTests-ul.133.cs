"Change Set:		8835CollectionsTests-ul.133CollectionsTests-ul.133:- a test for #utf8ToSqueak and Byte order markCollectionsTests-ar.129:Test for withNoLineLongerThan: bug.CollectionsTests-nice.130:Use literal ByteArrayCollectionsTests-nice.131:use ByteArray literalsCollectionsTests-nice.132:A test for #withSqueakLineEndings and for #withUnixLineEndings"!!StringTest methodsFor: 'tests - converting' stamp: 'ul 1/29/2010 02:12'!testUtf8ToSqueakByteOrderMark	"Ensure that utf8ToSqueak ignores Byte order mark (BOM) just like UTF8TextConverter does"	{		#('ï»¿' '').		#('ï»¿abc' 'abc').		"Make sure that we remove inner BOMs for maximal compatibility with UTF8TextConverter"		#('abcï»¿' 'abc').		{ 'abï»¿cÓ'. 'abc', (Unicode value: 1234) asString }	} do: [ :each |		self assert: each first utf8ToSqueak = each second ]! !!StringTest methodsFor: 'as yet unclassified' stamp: 'nice 1/18/2010 14:54'!testEquality	self assert: 'abc' = 'abc' asWideString.	self assert: 'abc' asWideString = 'abc'.	self assert: (#[ 97 0 0 0 ] asString ~= 'a000' asWideString).	self assert: ('a000' asWideString ~= #[ 97 0 0 0 ] asString).	self assert: ('abc' sameAs: 'aBc' asWideString).	self assert: ('aBc' asWideString sameAs: 'abc').	self assert: (#[ 97 0 0 0 ] asString 						sameAs: 'Abcd' asWideString) not.	self assert: ('a000' asWideString sameAs: 					#[ 97 0 0 0 ] asString) not.! !!IntervalTest methodsFor: 'tests' stamp: 'nice 1/18/2010 18:07'!testAsInterval	"This is the same as newFrom:"	self shouldnt: [		self assert: (#(1 2 3) as: Interval) = (1 to: 3).		self assert: (#(33 5 -23) as: Interval) = (33 to: -23 by: -28).		self assert: (#[2 4 6] as: Interval) = (2 to: 6 by: 2).	] raise: Error.	self should: [#(33 5 -22) as: Interval]		raise: Error		description: 'This is not an arithmetic progression'! !!StringTest methodsFor: 'testing - internet' stamp: 'nice 1/18/2010 23:33'!testWithSqueakLineEndings	{		'abc' -> 'abc'.		'abc', String cr -> ('abc', String cr).		'abc', String lf -> ('abc', String cr).		'abc', String crlf -> ('abc', String cr).		String cr, 'abc' -> (String cr, 'abc').		String lf, 'abc' -> (String cr, 'abc').		String crlf, 'abc' -> (String cr, 'abc').		'abc', String cr, String cr, 'abc' -> ('abc', String cr, String cr, 'abc').		'abc', String lf, String lf, 'abc' -> ('abc', String cr, String cr, 'abc').		'abc', String crlf, String crlf, 'abc' -> ('abc', String cr, String cr, 'abc').		String cr, 'abc', String cr, String crlf, 'abc', String lf -> (String cr, 'abc', String cr, String cr, 'abc', String cr).		String lf, 'abc', String lf, String crlf, 'abc', String cr -> (String cr, 'abc', String cr, String cr, 'abc', String cr).	} do: [ :each |		self assert: each key withSqueakLineEndings = each value ]! !!StringTest methodsFor: 'testing - internet' stamp: 'nice 1/18/2010 23:35'!testWithUnixLineEndings	{		'abc' -> 'abc'.		'abc', String cr -> ('abc', String lf).		'abc', String lf -> ('abc', String lf).		'abc', String crlf -> ('abc', String lf).		String cr, 'abc' -> (String lf, 'abc').		String lf, 'abc' -> (String lf, 'abc').		String crlf, 'abc' -> (String lf, 'abc').		'abc', String cr, String cr, 'abc' -> ('abc', String lf, String lf, 'abc').		'abc', String lf, String lf, 'abc' -> ('abc', String lf, String lf, 'abc').		'abc', String crlf, String crlf, 'abc' -> ('abc', String lf, String lf, 'abc').		String cr, 'abc', String cr, String crlf, 'abc', String lf -> (String lf, 'abc', String lf, String lf, 'abc', String lf).		String lf, 'abc', String lf, String crlf, 'abc', String cr -> (String lf, 'abc', String lf, String lf, 'abc', String lf).	} do: [ :each |		self assert: each key withUnixLineEndings = each value ]! !!StringTest methodsFor: 'tests - converting' stamp: 'nice 1/18/2010 18:10'!testUtf8ToSqueakLeadingChar	"Ensure utf8ToSqueak inserts the leading char just like UTF8TextConverter does"	| data |	data := #[ 227 129 130 227 129 132 227 129 134 227 129 136 227 129 138 ] asString "aiueo in Japanese".	self assert: data utf8ToSqueak = (data convertFromEncoding: #utf8)! !!StringTest methodsFor: 'tests - converting' stamp: 'ar 1/11/2010 20:00'!testWithNoLineLongerThan	"self run: #testWithNoLineLongerThan"	self assert: ('Hello World' withNoLineLongerThan: 5) = ('Hello', String cr, 'World').	self shouldnt:[('Hello', String cr, String cr,'World') withNoLineLongerThan: 5] raise: Error.! !!WideStringTest methodsFor: 'tests - compare' stamp: 'nice 1/18/2010 14:55'!testEqual	"from johnmci at http://bugs.squeak.org/view.php?id=5331"		self assert: 'abc' = 'abc'.	self assert: 'abc' = 'abc' asWideString.	self assert: 'abc' asWideString = 'abc'.	self assert: 'abc' asWideString = 'abc' asWideString.	self assert: ('abc' = 'ABC') not.	self assert: ('abc' = 'ABC' asWideString) not.	self assert: ('abc' asWideString = 'ABC') not.	self assert: ('abc' asWideString = 'abc' asWideString).	self assert: (#[ 97 0 0 0 ] asString ~= 'a000' asWideString).	self assert: ('a000' asWideString ~= #[ 97 0 0 0 ] asString).! !!WideStringTest methodsFor: 'tests - compare' stamp: 'nice 1/18/2010 14:56'!testSameAs	"from johnmci at http://bugs.squeak.org/view.php?id=5331"	self assert: ('abc' sameAs: 'aBc' asWideString).	self assert: ('aBc' asWideString sameAs: 'abc').	self assert: (#[ 97 0 0 0 ] asString sameAs: 'Abcd' asWideString) not.	self assert: ('a000' asWideString sameAs: #[ 97 0 0 0 ] asString) not.	! !