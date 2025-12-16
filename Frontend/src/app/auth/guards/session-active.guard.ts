import { CanMatchFn, Router} from '@angular/router';
import { inject } from '@angular/core';

export const sessionActiveGuard: CanMatchFn = (route, segments) => {  
  
  // Sesion activa quiere decir que puede ver el contenido de las paginas

  const userStr = localStorage.getItem('user');
  const router = inject(Router)

  if (userStr) {
    const user = JSON.parse(userStr);
  
    if (user.success) {      
      return true;
    } else {
      router.navigateByUrl('/auth')
      return false;
    }
  } else {
    router.navigateByUrl('/auth')
    return false;
  }
  
};
