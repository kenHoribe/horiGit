import cv2
import numpy as np
import os
import sys

def main():

    # 変数の定義
    input_path = 'SAMPLE/DVC00182.JPG'

    # 画像の読み込み
    image = cv2.imread(input_path)

    # 画像が読み込めなかった場合の例外処理
    if image is None:
        print(f'Failed to load image from {input_path}')
        return

    # 画像情報の表示
    print(f'data:\n{image}')
    print(f'type: {type(image)}')
    print(f'dtype: {image.dtype}')
    print(f'size: {image.size}')
    print(f'shape: {image.shape}')

def Learning001():
    img = cv2.imread("data/src/berry.jpg")
    cv2.imshow("img", img)
    cv2.waitKey(0)
    cv2.destroyAllWindows()
    
    os.mkdir("./output")
    cv2.imwrite("output/unko.jpg", img)
    
def Learning002():
    cap = cv2.VideoCapture("data/movie/Cosmos.mp4")
    if cap.isOpened() == False:
        sys.exit()
    
    ret, frame = cap.read()
    h, w = frame.shape[:2]
    fourcc = cv2.VideoWriter_fourcc(*"XVID")
    dst = cv2.VideoWriter("output/test.avi", fourcc, 30.0, (w,h))
    
    while True:
        ret, frame = cap.read()
        if ret == False:
            break
        cv2.imshow("img", frame)
        dst.write(frame)
        if cv2.waitKey(30) == 27:
            break
    cv2.destroyAllWindows()
    cap.release()    
        
def Learning003():
    img = cv2.imread("data/src/grapes.jpg")
    # 画像が読み込めなかった場合の例外処理
    if img is None:
        print(f'Failed to load image from {img}')
        return
    
    img_gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    img_hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
    
    cv2.imshow("img",img)
    cv2.imshow("gray",img_gray)

    # これもGrayScale
    #img_gray2 = cv2.imread("data/src/grapes.jpg",0)
    
    cv2.waitKey(0)
    cv2.destroyAllWindows()
        
if __name__ == '__main__':
    Learning003()
    #main()
    