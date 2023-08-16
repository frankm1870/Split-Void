# Split-Void Numbers
 
The Split-Void numbers are a number system made by me, Dane, and Sandrea, that defines division by zero via a new number, $u$, which is equal to $\frac{1}{0}$.  This system loses some properties present in the real numbers, namely the associative and distributive properties of multiplication.  This library implements the Split-Void numbers in C# with the following operations:

* Addition and subtraction
* Multiplication, following this form: $(a+bu)(c+du)=ac+(ad+bc+bd)u$ if $a,b,c,d$ are all non-zero, $a(b+cu)=ab+acu$, and if the first factor is $0$, then $0(a+bu)=b$
* Division, formed by taking the multiplicative inverse of the denominator following this formula: $(a+bu)^{-1}=\frac{1}{a}\left(1-\frac{b}{a+b}\cdot u\right)
* Raising to rational powers, with $v^{\frac{a}{b}}=\sqrt[b]{v^a}$
* Arbitrary functions with an input and output of a double, with $f{a+bu}=f(a)+f(a+b)u-f(a)u$
* A static method to parse expressions using Split-Void numbers (e.g. `SplitVoid.Parse("3+4v")`, note that `v` is used instead of `u` due to C# recognizing `u` as an unsigned number).

## Notes

Similar to the split-complex numbers, the number $1-2u$ in the Split-Void numbers squares to $1$.<br>
<br>
The first term in a Split-Void number is called the real part, and the second term is called the unfinite part.<br>
<br>
Some numbers do not have a multiplicative inverse, or have multiple multiplicative inverses:

* Numbers with an unfinite part of $1$ have an additional multiplicative inverse of $0$, in addition to the one given by the formula above.
* Numbers with a real part of $0$ or where the unfinite part is the negative of the real part have one less multiplicative inverse.
* Zero has an uncountably infinite number of multiplicative inverses, as $u+x$ is a multiplicative inverse for $x\in\mathbb{R}$.