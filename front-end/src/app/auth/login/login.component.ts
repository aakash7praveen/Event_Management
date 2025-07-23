import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { MatCardModule } from '@angular/material/card'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatInputModule } from '@angular/material/input'
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../services/auth.service';
import { SuccessDialogBoxComponent } from '../../shared/success-dialog-box/success-dialog-box.component';
import { ErrorDialogBoxComponent } from '../../shared/error-dialog-box/error-dialog-box.component';
import { MatDialogModule,MatDialog } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-login',
  imports: [ MatCardModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule, RouterModule, MatIconModule, MatDialogModule, CommonModule, MatProgressSpinnerModule],
  providers: [AuthService],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;
  content = {
    url : 'https://thumbs.dreamstime.com/b/team-designing-calendar-flat-vector-illustration-symbolizing-collaboration-event-planning-digital-organization-346322950.jpg'
  };

  constructor(private fb: FormBuilder, private authService: AuthService,private router: Router, private dialog: MatDialog) {
    this.loginForm = this.fb.group({
      email: ['',Validators.required, Validators.email],
      password: ['',Validators.required]
    });
  }

  isLoading = false;
  
  onSubmit() {
  if (this.loginForm.invalid) {
    this.loginForm.markAllAsTouched();
    return;
  }

  this.isLoading = true;

  this.authService.login(this.loginForm.value).subscribe({    
    next: (res) => {
      localStorage.setItem('currentUser', JSON.stringify(res));
      
      this.isLoading = false;
      
      if (res.role === true || res.role === 1) {
        this.router.navigate(['/dashboard']);
      } else {
        this.router.navigate(['/user-events']);
      }
    },
    error: () => {
      this.isLoading = false;
      this.dialog.open(ErrorDialogBoxComponent, {
        data: { message: '❌ Invalid email or password.' }
      });
    }
  });
}

}
