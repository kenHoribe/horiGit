s = 'one two three'
l = s.split()
print(l)

s = 'one two        three'
l = s.split()
print(l)

s = 'one\ttwo\tthree'
l = s.split()
print(l)

s = 'one,two,three'
l = s.split(',')
print(l)

s = 'one, two, three'
l = s.split(',')
print(l)

s = '  one  '
print(s.strip())

s = '-+-one-+-'
print(s.strip('-+'))

s = '-+- one -+-'
print(s.strip('-+'))

s = '-+- one -+-'
print(s.strip('-+ '))