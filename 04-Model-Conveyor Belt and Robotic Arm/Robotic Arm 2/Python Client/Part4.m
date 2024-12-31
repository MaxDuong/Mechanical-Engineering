clc;
clear all;
close all;
name = "Matlab";
Client = TCPInit('127.0.0.1',55002,name);

L1 = 0.0411; L2 = 0.192; L3 = 0.3163; L4 = 0.0759; L5 = 0.306; L6 = 0.233;
thetha1 = 0; thetha2 = 90; thetha3 = 0; thetha4 = 0; thetha5 = -90; thetha6 = 0;
alpha1 = 90; alpha2 = 0; alpha3 = 90; alpha4 = 270; alpha5 = 90; alpha6 = 0;
r1 = L1; r2 = L3; r3 = L4; r4 = 0; r5 = 0; r6 = 0; 
d1 = L2; d2 = 0; d3 = 0; d4 = L5; d5 = 0; d6 = L6;

L(1) = Revolute('d',d1,'a',r1,'alpha',alpha1*pi/180);
L(2) = Revolute('d',d2,'a',r2,'alpha',alpha2*pi/180);
L(3) = Revolute('d',d3,'a',r3,'alpha',alpha3*pi/180);
L(4) = Revolute('d',d4,'a',r4,'alpha',alpha4*pi/180);
L(5) = Revolute('d',d5,'a',r5,'alpha',alpha5*pi/180);
L(6) = Revolute('d',d6,'a',r6,'alpha',alpha6*pi/180);
robot = SerialLink(L);
joints = [thetha1*pi/180,thetha2*pi/180,thetha3*pi/180,thetha4*pi/180,thetha5*pi/180,thetha6*pi/180];
% robot.plot(joints);
% robot.teach(joints);

for i = 1 : 3

grab = 2; % activate EE (0 - release the object, 1 - grab, 2 - do nothing)

t = [0:0.1:2];

    X0 = 0.456; % - Y
    Y0 = 0.237; % - Z
    Z0 = 0.163; % - X
    
    T = transl(X0,-Y0, Z0) * trotx(180, "deg");
    qi1 = robot.ikine(T);
    qf1 = [thetha1*pi/180,thetha2*pi/180,thetha3*pi/180,thetha4*pi/180,thetha5*pi/180,thetha6*pi/180];
    q = jtraj(qf1,qi1,t);
    % robot.plot(q);
    
    b = 1;
    for a = 1 : length(q)
        func_data(Client, q, b);
        b=b+1;     
    end
    
    %% GRABING THE OBJECT
    
    % take object

    pause(0.5);
    grab = 1;
    func_grab(Client, grab);
    pause(4.5);
    
    % Generate Cube on the conveyor
    if (i == 1)
        init = 1;
        new_object(Client, init) % init: 1 - red, 2 - green, 3 - blue
    end
    if (i == 2)
        init = 3;
        new_object(Client, init) % init: 1 - red, 2 - green, 3 - blue
    end
    
    % back to initial pos
    b = 1;
    q = jtraj(qi1,qf1,t);
    % robot.plot(q);
    for a = 1 : length(q)
        func_data(Client, q, b); 
        b=b+1;
    end
    
    %% COLOR CHECK
    
    X1 = 0.333; % - cam
    Y1 = -0.068; % - cam
    Z1 = 0.2; % - cam
    
    T = transl(X1, -Y1, Z1) * trotx(180, "deg");
    qi1 = robot.ikine(T);
    qf1 = [thetha1*pi/180,thetha2*pi/180,thetha3*pi/180,thetha4*pi/180,thetha5*pi/180,thetha6*pi/180];
    q = jtraj(qf1,qi1,t);
    % robot.plot(q);
    
    b = 1;
    for a = 1 : length(q)
        func_data(Client, q, b);
        b=b+1;     
    end
    
    color = color_check(Client); % function for detecting colors
    
    if color == 1 % Red sorting
        X2 = 0.471;
        Y2 = -0.068;
        Z2 = 0.086+0.08;
    elseif color == 2 % Green sorting
        X2 = 0.471;
        Y2 = -0.212;
        Z2 = 0.086+0.12;
    else % Blue sorting
        X2 = 0.333;
        Y2 = -0.212;
        Z2 = 0.085+0.2;
    end
    
    pause(1);
    %% PLACING THE CUBE TO THE SORTED PLACE
    
    T = transl(X2, -Y2, Z2) * trotx(180, "deg");
    qf1 = robot.ikine(T);
    q = jtraj(qi1,qf1,t);
    % robot.plot(q);
    
    b = 1;
    for a = 1 : length(q)
        func_data(Client, q, b);
        b=b+1;     
    end
    
    % release object
    grab = 0;
    func_grab(Client, grab);
    pause(2.5);
    
    %% Back to initial pos
    b = 1;
    qi1 = [thetha1*pi/180,thetha2*pi/180,thetha3*pi/180,thetha4*pi/180,thetha5*pi/180,thetha6*pi/180];
    q = jtraj(qf1,qi1,t);
    % robot.plot(q);
    for a = 1 : length(q)
        func_data(Client, q, b); 
        b=b+1;
    end
end
%Close Gracefully
fprintf(1,"Disconnected from server\n");