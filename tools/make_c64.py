#!/usr/bin/env python

""" NOTE:
c64.bin contains both C64 character sets. Characters 0..255 for the uppercase
set, 256..511 for the lowercase one, plus graphic chars. The second part of
each of these chunks contains the reverse characters, e.g. 1+128 contains the
reverse of 1 (A), etc. We can omit these, since they can be trivially generated
if necessary. So we extract positions 0..127 and 256..383, where the second
block will end up in positions 128..255 in the generated C# code.

In other words, positions 1,2,.. hold A,B,.. (uppercase)
while positions 128,129,.. hold a,b,.. (lowercase)
Some graphic chars will have double entries, but that is irrelevant really.
"""

data = open("c64.bin", "rb").read()

bytesets = {}

for i in range(256):
    idx = i if i < 128 else i+128
    byteset = []
    pos = idx*8
    eight = data[pos:pos+8]
    for byte in eight:
        s = "{0:08b}".format(byte)
        bits = [int(x) for x in s]
        byteset.append(bits)
    bytesets[i] = byteset

INDENT = " "*8
print(INDENT + "int[,] chars = new int[256,64] {")
for i in range(256):
    print(f"{INDENT}    {{ // {i}")
    byteset = bytesets[i]
    for bits in byteset:
        print(INDENT + INDENT + (', '.join([str(bit) for bit in bits]) + ","))
    print(f"{INDENT}    }},")
print(INDENT + "};")
