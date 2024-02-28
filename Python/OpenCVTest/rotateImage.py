import cv2
import numpy as np

dir = r"D:\HogeGit\horiGit\Python\OpenCVTest\ImageDir\Feature001" + "\\"
filename = r"crop_test_1.jpg"
path = dir + filename
img = cv2.imread(path)

height, width, _ = img.shape  # 形状取得 
center = (int(width/2), int(height/2))  # 中心座標設定

img_gray = cv2.imread(path, 0)  # グレースケール読み出し
ret, img_binary = cv2.threshold(img_gray, 180, 255, cv2.THRESH_BINARY)  # 輝度180を境界に二値化
contours, hierarchy = cv2.findContours(img_binary, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)  # 領域検出　

# 各領域を読み出し
angle = 0
for i in contours:
    area = cv2.contourArea(i)  # 各領域の面積
    
    if 10000 < area < 15000:  # 10000<面積<15000の場合
        rect = cv2.minAreaRect(i)  # 回転外接矩形の算出
        angle = rect[2]  # 回転角を設定
        print("Angle:" + str(angle))

        trans = cv2.getRotationMatrix2D(center, angle, scale=1)  # 変換行列の算出
        img2 = cv2.warpAffine(img, trans, (width, height))  # 元画像を回転

        #画像の表示
        cv2.imshow('imshow_test', img2)
        cv2.waitKey(0)
        cv2.destroyAllWindows()
    else:
        rect = cv2.minAreaRect(i)  # 回転外接矩形の算出
        angle = rect[2]  # 回転角を設定
        #print("Angle:" + str(angle))

        trans = cv2.getRotationMatrix2D(center, angle, scale=1)  # 変換行列の算出
        img2 = cv2.warpAffine(img, trans, (width, height))  # 元画像を回転

        #画像の表示
        #cv2.imshow('imshow_test', img2)
        #cv2.waitKey(0)
        #cv2.destroyAllWindows()


