# Split-Void Numbers
 
The Split-Void numbers are a number system made by me, Dane, and Sandrea, that defines division by zero via a new number, $u$, which is equal to $\frac{1}{0}$.  This system loses some properties present in the real numbers, namely the associative and distributive properties of multiplication.  This library implements the Split-Void numbers in C# with the following operations:

* Addition and subtraction
* Multiplication, following this form: $(a+bu)(c+du)=ac+(ad+bc+bd)u$ if $a,b,c,d$ are all non-zero, $a(b+cu)=ab+acu$, and if the first factor is $0$, then $0(a+bu)=b$
* Division, formed by taking the multiplicative inverse of the denominator following this formula: $(a+bu)^{-1}=\frac{1}{a}\left(1-\frac{b}{a+b}\cdot u\right)$
* Raising to rational powers, with $v^{\frac{a}{b}}=\sqrt[b]{v^a}$
* Arbitrary functions with an input and output of a double, with $f{a+bu}=f(a)+f(a+b)u-f(a)u$
* A static method to parse expressions using Split-Void numbers (e.g. `SplitVoid.Parse("3+4v")`, note that `v` is used instead of `u` due to C# reserving `u` for unsigned numbers).

## Notes

Similar to the split-complex numbers, the number $1-2u$ in the Split-Void numbers squares to $1$.<br>
<br>
The first term in a Split-Void number is called the real part, and the second term is called the unfinite part.<br>
<br>
Some numbers do not have a multiplicative inverse, or have multiple multiplicative inverses:

* Numbers with an unfinite part of $1$ have an additional multiplicative inverse of $0$, in addition to the one given by the formula above.
* Numbers with a real part of $0$ or where the unfinite part is the negative of the real part have one less multiplicative inverse.
* Zero has an uncountably infinite number of multiplicative inverses, as $u+x$ is a multiplicative inverse for $x\in\mathbb{R}$ (any real value of $x$).

# Details

The Split-Void number system defines division by zero, but in doing so loses a few properties that are useful to have, namely that multiplication is not associative: $0(uu)\neq(0u)u$, (note that $u^2=u$) and that multiplication is not distributive: $u(0+0)\neq 0u+0u$.  These issues don't occur in most scenarios however, namely when $0$ and $u$ aren't in the same term, and aren't in the denominators of any fractions.  In these cases multiplication works as expected.<br>
<br>
As mentioned in the notes section, the Split-Void numbers have a number analogous to $j$ in the split-complex numbers.  The split-complex numbers are a system created by assuming a square root of $1$ that isn't itself $1$ or $-1$, and calling this number $j$.  In the Split-Void numbers, no new constant needs to be defined, as $(1-2u)^2=1$.  This number doesn't act exactly as $j$, since multiplying $j$ by $0$ gives $0$ as normal, but $0(1-2u)=-2$.  This discovery led to the renaming of the system from the Zero-Divisive numbers to the Split-Void numbers, with void coming from how often $0$ is relevant in the system.