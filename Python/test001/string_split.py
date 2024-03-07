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

s = 'one, two,  three'
l = [x.strip() for x in s.split(',')]
print(l)
#ここでは、split()で文字列を分割して取得したリストにstrip()を適用する。