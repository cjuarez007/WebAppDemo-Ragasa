import { CanMatchFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const sessionInactiveGuard: CanMatchFn = (route, segments) => {
    // Sesion inactiva quiere decir que no puede ver el contenido de las paginas

    const userStr = localStorage.getItem('user');
    const router = inject(Router)
  
    if (userStr) {
      const user = JSON.parse(userStr);
    
      if (!user.success) {
        
        return true;
      } else {
        router.navigateByUrl('/main')
        return false;
      }
    } else {      
      return true;
    }    
};
