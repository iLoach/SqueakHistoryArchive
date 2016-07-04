"Change Set:		9748System-ar.297System-ar.297:Add license notice from 4.0 release.System-laza.295:Fix missing .System-nice.296:Let nextPut: answer the put object.Same for nextPutAll:, answer the collection argument"!!SystemDictionary methodsFor: 'license' stamp: 'cbr 3/9/2010 22:22'!license	"This method contains the text of the license agreement for Squeak."	^ 'Copyright (c) The individual, corporate, and institutional contributors who have collectively contributed elements to this software ("The Squeak Community"), 1996-2010 All rights reserved.Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS INTHE SOFTWARE.Portions of Squeak are covered by the following licenseCopyright (c) Xerox Corp. 1981, 1982 All rights reserved.Copyright (c) Apple Computer, Inc. 1985-1996 All rights reserved.Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at  http://www.apache.org/licenses/LICENSE-2.0Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.'! !!DummyStream methodsFor: 'accessing' stamp: 'nice 3/19/2010 19:03'!nextPut: aByte	"do nothing"	^aByte! !!NaturalLanguageTranslator class methodsFor: 'file-services' stamp: 'laza 3/19/2010 14:01'!mergeTranslationFileNamed: fileFullNameString 	"merge the translation in the file named fileFullNameString"	FileStream readOnlyFileNamed: fileFullNameString do: [:stream |		| localeID translator |		localeID := LocaleID isoString: stream localName sansPeriodSuffix.		translator := self localeID: localeID.		translator loadFromStream: stream].	LanguageEnvironment resetKnownEnvironments.! !