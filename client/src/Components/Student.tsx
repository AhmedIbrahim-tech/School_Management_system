import { Component } from "react";
import { Button, ButtonToolbar, Table, Spinner, Alert } from "react-bootstrap";
import { AddStudentModal } from "./AddStudentModal";
import { EditStudentModal } from "./EditStudentModal";
import StudentService from "../Services/StudentService";
import { HomeState } from "../Interface/DashboardCard";



export class Student extends Component<{}, HomeState> {
  constructor(props: {}) {
    super(props);
    this.state = {
      students: [],
      loading: true,
      error: "",
      addModalShow: false,
      editModalShow: false,
      selectedStudent: null,
    };
  }

  componentDidMount() {
    this.fetchStudents();
  }

  fetchStudents = async () => {
    try {
      const students = await StudentService.getAllStudents();
      this.setState({ students, loading: false });
    } catch (error) {
      this.setState({
        error: error instanceof Error ? error.message : "Unknown error",
        loading: false,
      });
    }
  };

  deleteStudent = async (id: number) => {
    if (!window.confirm("Are you sure you want to delete this student?")) return;

    try {
      await StudentService.deleteStudent(id);
      this.fetchStudents();
    } catch (error) {
      this.setState({
        error: error instanceof Error ? error.message : "Delete failed",
      });
    }
  };

  render() {
    const { students, loading, error, addModalShow, editModalShow, selectedStudent } = this.state;

    return (
      <div className="container mt-4">
        <h2 className="mb-4">Students Management</h2>

        {error && <Alert variant="danger">{error}</Alert>}

        <div className="mb-3 d-flex justify-content-end">
          <Button
            variant="success"
            onClick={() => this.setState({ addModalShow: true })}
          >
            Add New Student
          </Button>
        </div>

        {loading ? (
          <div className="text-center">
            <Spinner animation="border" role="status">
              <span className="visually-hidden">Loading...</span>
            </Spinner>
          </div>
        ) : (
          <Table striped bordered hover responsive>
            <thead className="table-dark">
              <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Address</th>
                <th>Phone</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {students.map((student) => (
                <tr key={student.studID}>
                  <td>{student.studID}</td>
                  <td>{student.name}</td>
                  <td>{student.address}</td>
                  <td>{student.phone}</td>
                  <td>
                    <ButtonToolbar>
                      <Button
                        variant="outline-primary"
                        className="me-2"
                        onClick={() =>
                          this.setState({
                            editModalShow: true,
                            selectedStudent: student,
                          })
                        }
                      >
                        Edit
                      </Button>
                      <Button
                        variant="outline-danger"
                        onClick={() => this.deleteStudent(student.studID)}
                      >
                        Delete
                      </Button>
                    </ButtonToolbar>
                  </td>
                </tr>
              ))}
            </tbody>
          </Table>
        )}

        {/* Modals */}
        <AddStudentModal
          show={addModalShow}
          onHide={() => this.setState({ addModalShow: false })}
          onSuccess={() => {
            this.setState({ addModalShow: false });
            this.fetchStudents();
          }}
        />

        {selectedStudent && (
          <EditStudentModal
            show={editModalShow}
            student={selectedStudent}
            onHide={() => this.setState({ editModalShow: false, selectedStudent: null })}
            onSuccess={() => {
              this.setState({ editModalShow: false });
              this.fetchStudents();
            }}
          />
        )}
      </div>
    );
  }
}
