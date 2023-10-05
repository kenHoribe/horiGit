#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
}

MainWindow::~MainWindow()
{
    delete ui;
}


void MainWindow::on_pushButton_clicked()
{

}


void MainWindow::on_pushButton_2_clicked()
{

}


void MainWindow::on_pushButton_3_clicked()
{

}


// 押下したキー名をデバッグ出力する
// その他のキーは、16進数を出力する
void MainWindow::keyPressEvent(QKeyEvent *event )
{
    if( event->key() == Qt::Key_A )
    {
        ui->pushButton_5->setText("GH");
    }
}
