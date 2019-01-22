close all;
clear all;
fileID=fopen('Global srednieActimel2.txt')
formatSpec = '%f'

pr=fscanf(fileID,formatSpec);
fileID=fopen('Local srednieActimel2.txt')
prl=fscanf(fileID,formatSpec);
figure(1);

hold on;
x = 1:100;
X = x(:);
Y = pr(:,1);
Z = prl(:,1);
ylim([0 200])
L=plot(X,Z);
G=plot(X,Y);
G.LineWidth=1.3;
L.LineWidth=1.3;
ylabel('Funkcja celu','FontSize',15);
xlabel('iteracja','FontSize',15);
legend('\fontsize{13} Rozwi¹zanie globalne','\fontsize{13} Rozwi¹zanie lokalne')
grid on ;
hold on




