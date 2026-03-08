
s: secret codeword  ( example 🟢🟠🟢🟣 )
P: all possible codewords

P = { ⚫⚫⚫⚫, ⚫⚫⚫🟣, …, 🟠🟠🟠🟠 }

|P| = 6^4 = 1296 

g: first guess = ⚫⚫🟣🟣

r: first result = s ⨂ g    ( example ₀₁ )


R = { {ki…|K|, ki ⨂  g = 04 }, { {ki…|K| ki ⨂  g = 03 }, … {ki…|K| ki ⨂ g = 40 }} ( example for ₀₁, |K| = 256 )

R₀₁ = { ⚫🟢🟢🟢, ⚫🟢🟢🔵, …, 🟢⚫🟢🟢, …, 🟠🟠🟣🟠 }

K = P - {p1…|P|, pi ⨂  g ≠ r }  ( example remove { ⚫⚫⚫⚫, ⚫⚫⚫🟣, …, 🔴🔴🔴🔴, …, 🔵🔵🔵🔵, … }, but not { …, ⚫🔴🔴🔴, … , 🔴🔴🔴🟣, … }

K: all the codewords that match g with same result as r = remaining possible solutions
