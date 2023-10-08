#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    dlg = new ReplaceDialog(this);

    connect(dlg, SIGNAL(LotWaferIdUpdated()), this, SLOT(FuncA()));
    connect(this, SIGNAL(temporaryEvent()), this, SLOT(FuncB()));
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::FuncA()
{
    ui->lineEdit->setText("DONE!");
}

void MainWindow::FuncB()
{
    ui->lineEdit->setText("HOGE");
}

void MainWindow::on_pushButton_clicked()
{
    emit temporaryEvent();
    dlg->exec();
}

