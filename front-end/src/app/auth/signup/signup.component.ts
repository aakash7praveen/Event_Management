import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { SuccessDialogBoxComponent } from '../../shared/success-dialog-box/success-dialog-box.component';
import { ErrorDialogBoxComponent } from '../../shared/error-dialog-box/error-dialog-box.component';
import { Router } from '@angular/router';
import { MatDialogModule,MatDialog } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-signup',
  imports: [ MatCardModule, MatFormFieldModule, MatInputModule, MatIconModule, ReactiveFormsModule, MatDialogModule, SuccessDialogBoxComponent, CommonModule, ErrorDialogBoxComponent],
  providers: [AuthService],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.scss'
})
export class SignupComponent {
  signupForm: FormGroup;
  
  constructor(private fb: FormBuilder,
              private dialog: MatDialog,
              private router: Router,
              private authService: AuthService,
              private http: HttpClient
  ) {
    this.signupForm = this.fb.group({
      firstName: this.fb.control('', Validators.required),
      middleName: this.fb.control(''),
      lastName: this.fb.control('', Validators.required),
      email: this.fb.control('', [Validators.required, Validators.email]),
      phoneNumber: this.fb.control('', [Validators.pattern(/^\d{10}$/) ]),
      password: this.fb.control('', [Validators.required, Validators.minLength(8), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*?~_]).{8,}$')]),
      confirmPassword: this.fb.control('', Validators.required)
    });
  }

  get passwordMismatch(): boolean {
    const pwd = this.signupForm.get('password')?.value;
    const confirmPwd = this.signupForm.get('confirmPassword')?.value;
    return pwd && confirmPwd && pwd !== confirmPwd;
  }

  selectedFileName: string | null = null;
  uploadedFilePath: string = '';

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (!file) return;

    this.selectedFileName = file.name;

    const formData = new FormData();
    formData.append('file', file);

    this.http.post<any>('https://localhost:7287/api/users/upload-profile-picture', formData)
      .subscribe({
        next: (res) => {
          this.uploadedFilePath = res.filePath;
          console.log('✅ Image uploaded successfully:', this.uploadedFilePath);
        },
        error: () => {
          this.dialog.open(ErrorDialogBoxComponent, {
            data: { message: '❌ Upload failed. Please try again.' }
          });
        }
      });
  }


  onSubmit() {
    console.log(this.signupForm.value);
    console.log(this.signupForm.valid);

    if (this.signupForm.invalid) {
      this.signupForm.markAllAsTouched();
      return;
    }

    if (this.signupForm.value.password !== this.signupForm.value.confirmPassword) {
      console.log("Password mismatch");
      return;
    }

    const signupData = {
      ...this.signupForm.value,
      profilePicture: this.uploadedFilePath 
    };

    this.authService.signup(signupData).subscribe({
      next: () => {
        const dialogRef = this.dialog.open(SuccessDialogBoxComponent, {
          data: { message: '🎉 Congratulations! Your account has been created successfully.' }
        });

        dialogRef.afterClosed().subscribe(() => {
          this.router.navigate(['/login']);
        });
      },
      error: (err) => {
        console.error("Signup failed:", err);
        this.dialog.open(ErrorDialogBoxComponent, {
          data: { message: 'Signup failed. Please try again later.' }
        });
      }
    });
  }
}

