"Change Set:		9663VersionNumber-ar.2VersionNumber-ar.2:Get rid of a few *smbase extensions/overrides."!!VersionNumber methodsFor: 'printing' stamp: 'svp 6/18/2002 17:23'!versionStringOn: strm	| first |	first := true.	numbers do: [ :ea |		first ifFalse: [strm nextPut: $.].		first := false.		ea printOn: strm]	! !!VersionNumber methodsFor: 'printing' stamp: 'gk 1/23/2004 10:13'!versionString	^String streamContents: [ :strm | self versionStringOn: strm ]! !