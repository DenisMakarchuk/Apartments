import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CommentUserService, PagedResponseOfCommentDTO } from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-own-comments',
  templateUrl: './own-comments.component.html',
  styleUrls: ['./own-comments.component.scss']
})
export class OwnCommentsComponent implements OnInit {

  spinning = false;
  errorMessage: string;

  response: PagedResponseOfCommentDTO;
  requestForm: FormGroup;
  pages: number[] = [];

  constructor(
    private commentService: CommentUserService,
    private fb: FormBuilder
  ) { 
    this.requestForm = this.fb.group({
      pageNumber: 1,
      pageSize: 20
    });
  }

  ngOnInit(): void {
  }

  getOwnComments(){
    this.errorMessage = null;
    this.spinning = true;

    this.commentService.getAllCommentsByAuthorId(this.requestForm.value)
    .subscribe(response => {

      this.spinning = false;
      this.response = response;
    
      this.pages = [];
      for (let index = 1; index <= response.totalPages; index++) {
        this.pages.push(index);
      }
    },
    error=>{
      this.spinning = false;
 
      if (error.status ===  500) {
        this.errorMessage = "Error 500: Internal Server Error";
      }
      if (error.status ===  400) {
        this.errorMessage = "Error 400: " + error.response;
      }
      if (error.status ===  403) {
        this.errorMessage = "Error 403: You are not authorized";
      }
      else{
        this.errorMessage = "An error occurred.";
      }
    });
  }

  previousePage(){
    if (this.response != null && this.response.hasPreviousPage) {
      this.requestForm.value.pageNumber -= 1;
      this.getOwnComments();
    }
  }

  nextPage(){
    if (this.response != null && this.response.hasNextPage) {
      this.requestForm.value.pageNumber += 1;
      this.getOwnComments();
    }
  }

  getPage(page: number){
    if (this.response != null && page > 0 && page <= this.response.totalPages) {
      this.requestForm.value.pageNumber = page;
      this.getOwnComments();
    }
  }

}
