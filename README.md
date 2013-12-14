JVMdotNET
=========

Implemetace Java Virtual Machine v prostředí .NET

Co se povedlo:

* Implementována téměr celý bytecode výjma instrukcí:
  * monitorenter / monitorexit (tzn. nejsou podporovány thready)
  * invokedynamic 
  * checkcast / instanceof
  
* Nativní implementace vybraných tříd z java class library (implementace nejsou kompletní, jen to co bylo třeba - tzn. například na Objectu není metoda getClass, ale třeba hashCode ano.
  * java.lang.Object
  * java.lang.String
  * java.lang.System
  * java.io.PrintStream
  * Exception třídy:
    * java.lang.Throwable
    * java.lang.Exception
    * java.lang.RuntimeException
    * java.lang.ArithmeticException
    * java.lang.ArrayIndexOutOfBoundsException
    * java.lang.NegativeArraySizeException
    * java.lang.NullPointerException
* Podpora vyjímek - výše uvedené třídy vyjímek vychazuje sama JVM při vykonávání bytecodu
* Inline method cache - řešeno pomocí "rozkopírování" method z předků do potomků (viz metoda Resolve na třídě JavaClass v implementaci)
* Podpora statických inicializátorů (konstruktorů)
 

Co se nepovedlo:
* Nejsou thready...
* Není garbage collection...
* U vyjímek není podpora pro podtřídy Error - například dojde-li k vyjímce ve statickém konstruktoru měl by se správně vyhodit ExceptionInInitializerError přímo do JVM a tedy máte mít možnost ho catchnout. My ovšem vyhodíme .NET vyjímku a ukončíme celý běh.



Obsah:
* Součástí je jednak java implementace problému batohu (ve složce Knapsack) jak zdrojáky tak zkompilovae class soubory
* Dále pak implementace samotné JVM v .NET, případně je k dispozici i zkompilovaná verze

Příklad s batohem lze pak spustit takto:

JVMdotNET.Console.exe RUNKnapsack.class Knapsack.class Algorithm.class

