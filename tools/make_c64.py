#!/usr/bin/env python

data = open("c64.bin", "rb").read()

bytesets = {}

for i in range(256):
    byteset = []
    pos = i*8
    eight = data[pos:pos+8]
    for byte in eight:
        s = "{0:08b}".format(byte)
        bits = [int(x) for x in s]
        byteset.append(bits)
    bytesets[i] = byteset

for i in range(256):
    print(f"    {{ // {i}")
    byteset = bytesets[i]
    for bits in byteset:
        print(" "*8 + (', '.join([str(bit) for bit in bits])))
    print("    },")
