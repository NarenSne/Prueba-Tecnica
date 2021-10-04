import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  form: FormGroup;
  baseUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private spinner: NgxSpinnerService) {
    this.baseUrl = baseUrl;
  }
  edit = false;
  usuarios: Usuario[];
  ngOnInit(): void {
    this.obtenerUsuarios();
    this.form = new FormGroup({
      nombre: new FormControl('', [Validators.required, Validators.pattern('[a-zA-Z ]*'), Validators.maxLength(50)]),
      apellidos: new FormControl('', [Validators.required, Validators.pattern('[a-zA-Z ]*'), Validators.maxLength(50)]),
      fechaNac: new FormControl('', [Validators.required]),
      fotoUsuario: new FormControl('', [Validators.required]),
      estadoCivil: new FormControl('', [Validators.required]),
      tieneHermanos: new FormControl('', [Validators.required])
    })
  }
  submit() {
    if (!this.edit) {
      let valores: Usuario = this.form.value;
      valores.estadoCivil = parseInt(valores.estadoCivil.toString());
      valores.fotoUsuario = this.base64String.toString();
      valores.tieneHermanos != true ? valores.tieneHermanos = false : null;
      this.spinner.show();
      this.http.post<Usuario>(this.baseUrl + 'api/usuarios/AgregarUsuario', valores).subscribe(result => {
        this.base64String = "";
        this.spinner.hide();
        this.obtenerUsuarios()
      }, error => console.error(error));
    } else {
      let valores: Usuario = this.form.value;
      valores.id = this.user.id;
      valores.estadoCivil = parseInt(valores.estadoCivil.toString());
      valores.fotoUsuario = this.base64String.toString();
      valores.tieneHermanos != true ? valores.tieneHermanos = false : null;
      this.spinner.show();
      this.http.post<Usuario>(this.baseUrl + 'api/usuarios/EditarUsuario/' + valores.id, valores).subscribe(result => {
        this.form.reset()
        this.spinner.hide();
        this.obtenerUsuarios()
      }, error => console.error(error));
    }
    
  }
  obtenerUsuarios() {
    this.spinner.show();
    this.http.get<Usuario[]>(this.baseUrl + 'api/usuarios/ObtenerUsuarios').subscribe(result => {
      this.usuarios = result;
      this.spinner.hide();
    }, error => console.error(error));
  }
  base64String: string| ArrayBuffer = "";
  handleFileInput(files: FileList) {
    let me = this;
    let file = files[0];
    let reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
      console.log(reader.result);
      me.base64String = reader.result;
    };
    reader.onerror = function (error) {
      console.log('Error: ', error);
    };
  }
  estadoCivil(estado) {
    switch (estado) {
      case 0: return "Soltero"
        break;
      case 1: return "Casado"
        break;
      case 2: return "Divociado"
        break;
      case 3: return "Union Libre"
        break;

    }
  }
  eliminar(userId) {
    this.spinner.show();
    this.http.get<Usuario>(this.baseUrl + 'api/usuarios/EliminarUsuario/' + userId).subscribe(result => {
      this.spinner.hide();
      this.obtenerUsuarios()
    }, error => console.error(error));
  }
  user: Usuario = {
    "id": null,
    "nombre": "",
    "apellidos": "",
    "fechaNac": "",
    "fotoUsuario": "",
    "estadoCivil": 0,
    "tieneHermanos": false
  };
  editar(user) {
    this.user = user;
    this.base64String = this.user.fotoUsuario;
    this.edit = true;
  }
}

interface Usuario { nombre: String, apellidos: String, fechaNac: String, fotoUsuario: any, estadoCivil: number, tieneHermanos: boolean, id:number }
