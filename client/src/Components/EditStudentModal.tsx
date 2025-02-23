import { Modal, Form, Button, Spinner, Alert } from "react-bootstrap";
import { useState, useEffect } from "react";
import { UpdateStudent } from "../Interface/Student";
import { EditStudent } from "../Interface/StudentModal";
import StudentService from "../Services/StudentService";

export const EditStudentModal = ({ show, student, onHide, onSuccess }: EditStudent) => {
  const [formData, setFormData] = useState<UpdateStudent>({
    id: student?.studID || 0,
    nameAr: student?.name || "",
    nameEn: student?.name || "",
    address: student?.address || "",
    phone: student?.phone || "",
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (student) {
      setFormData({
        id: student.studID,
        nameAr: student.name,
        nameEn: student.name,
        address: student.address,
        phone: student.phone,
      });
    }
  }, [student]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError("");

    try {
      await StudentService.updateStudent(formData.id, formData);
      onSuccess();
      onHide();
    } catch (err) {
      setError(err instanceof Error ? err.message : "An error occurred");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal show={show} onHide={onHide} centered>
      <Modal.Header closeButton>
        <Modal.Title>Edit Student</Modal.Title>
      </Modal.Header>
      <Form onSubmit={handleSubmit}>
        <Modal.Body>
          {error && <Alert variant="danger">{error}</Alert>}

          <Form.Group className="mb-3">
            <Form.Label>Name (Arabic)</Form.Label>
            <Form.Control
              type="text"
              name="nameAr"
              value={formData.nameAr}
              onChange={handleChange}
              required
            />
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Name (English)</Form.Label>
            <Form.Control
              type="text"
              name="nameEn"
              value={formData.nameEn}
              onChange={handleChange}
              required
            />
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Address</Form.Label>
            <Form.Control
              type="text"
              name="address"
              value={formData.address}
              onChange={handleChange}
              required
            />
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Phone</Form.Label>
            <Form.Control
              type="text"
              name="phone"
              value={formData.phone}
              onChange={handleChange}
              required
            />
          </Form.Group>

        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={onHide} disabled={loading}>
            Cancel
          </Button>
          <Button variant="primary" type="submit" disabled={loading}>
            {loading ? <Spinner size="sm" /> : "Save Changes"}
          </Button>
        </Modal.Footer>
      </Form>
    </Modal>
  );
};