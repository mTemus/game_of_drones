using System;

[Serializable]
public enum MoveMachine 
{
    Start, Stop, 
    
    // Flying Vertically
        // Y0
            FlyVerticallyUp, FlyVerticallyDown,
            
        // Y+
            // X+
            FlyVerticallyToXPlusOnYPlus,
            
            // X-
            FlyVerticallyToXMinusOnYPlus,
            
            // Z
            FlyVerticallyToZPlusOnYPlus,
            
            // Z-
            FlyVerticallyToZMinusOnYPlus,
            
            // X and Z
            FlyVerticallyToXPlusZPlusOnYPlus,
            FlyVerticallyToXPlusZMinusOnYPlus,
            FlyVerticallyToXMinusZPlusOnYPlus,
            FlyVerticallyToXMinusZMinusOnYPlus,
            
        // Y-
            // X+
            FlyVerticallyToXPlusOnYMinus,
                
            // X-
            FlyVerticallyToXMinusOnYMinus,
                
            // Z
            FlyVerticallyToZPlusOnYMinus,
                
            // Z-
            FlyVerticallyToZMinusOnYMinus,
                
            // X and Z
            FlyVerticallyToXPlusZPlusOnYMinus,
            FlyVerticallyToXPlusZMinusOnYMinus,
            FlyVerticallyToXMinusZPlusOnYMinus,
            FlyVerticallyToXMinusZMinusOnYMinus,
    
    
    // Flying Horizontally
        // Y
            // X
            FlyHorizontallyToXPlusOnYZero,
            
            // X-
            FlyHorizontallyToXMinusOnYZero,
            
            // Z
            FlyHorizontallyToZPlusOnYZero,
            
            // Z-
            FlyHorizontallyToZMinusOnYZero,
            
            // X and Z
            FlyHorizontallyToXPlusZPlusOnYZero,
            FlyHorizontallyToXPlusZMinusOnYZero,
            FlyHorizontallyToXMinusZPlusOnYZero,
            FlyHorizontallyToXMinusZMinusOnYZero,

    // Lights
    LightOn, LightOff,
    
    // Angles - X
        // X+
        HalfCircleToXPlusOnZMinus, HalfCircleToXPlusOnZPlus,
        HalfCircleToXPlusOnYMinus, HalfCircleToXPlusOnYPlus,
        // X-
        HalfCircleToXMinusOnYMinus, HalfCircleToXMinusOnYPlus,
        HalfCircleToXMinusOnZMinus, HalfCircleToXMinusOnZPlus,
    
    // Angles - Y
        // Y+
        HalfCircleToYPlusOnXMinus, HalfCircleToYPlusOnxPlus,  
        HalfCircleToYPlusOnZMinus, HalfCircleToYPlusOnZPlus,
        // Y-
        HalfCircleToYMinusOnXMinus, HalfCircleToYMinusOnXPlus,  
        HalfCircleToYMinusOnZMinus, HalfCircleToYMinusOnZPlus,
    
    // Angles - Z
        // Z+
        HalfCircleToZPlusOnXMinus, HalfCircleToZPlusOnXPlus,
        HalfCircleToZPlusOnYMinus, HalfCircleToZPlusOnYPlus,
        // Z-
        HalfCircleToZMinusOnXMinus, HalfCircleToZMinusOnXPlus, 
        HalfCircleToZMinusOnYMinus, HalfCircleToZMinusOnYPlus,
    
    
    // Self rotation (?)
    // Full Circles
    // Spirals
    // ??
}
